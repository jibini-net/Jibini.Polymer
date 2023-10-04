// Copyright (c) 2023 Zach Goethel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is furnished
// to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in 
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.

#include "fsa.h"

#include <stdlib.h>
#include <stdbool.h>
#include <string.h>

dfa_jump_t *find_dfa_action(fsa_node_t *fsa, char letter)
{
    for (dfa_jump_t *it = fsa->actions; it; it = it->_next)
    {
        if (it->letter == letter)
        {
            return it;
        }
    }
    return NULL;
}

void exec_nfa_actions(fsa_node_t *fsa, nfa_action_it_fun it_fun, void *it_fun_args)
{
    it_fun(fsa, it_fun_args);
    for (nfa_jump_t *it = fsa->eps; it; it = it->_next)
    {
        exec_nfa_actions(it->node, it_fun, it_fun_args);
    }
}

void _build_closure_it(fsa_node_t *eps, void *args)
{
    // Deref x1: moves end-of-list pointer
    // Deref x2: reassigns "next" on caboose node
    nfa_jump_t ***tail = (nfa_jump_t ***)args;

    nfa_jump_t *node = (nfa_jump_t *)malloc(sizeof(nfa_jump_t));
    *node = (nfa_jump_t){0};
    node->node = eps;
    **tail = node;

    *tail = &node->_next;
}

eps_closure_t epsilon_closure(fsa_node_t *fsa)
{
    eps_closure_t result = {0};

    // End-of-list pointer referenced in iterator function
    nfa_jump_t **tail = &result.closure;
    exec_nfa_actions(fsa, _build_closure_it, &tail);

    return result;
}

eps_closure_t epsilon_closure_on(fsa_node_t *fsa, char letter)
{
    eps_closure_t result = {0};

    dfa_jump_t *adjacent = find_dfa_action(fsa, letter);
    if (adjacent)
    {
        result = epsilon_closure(adjacent->node);
    }

    return result;
}

nfa_jump_t *build_nfa(fsa_node_t *fsa, char *pattern, int32_t accept)
{
    nfa_jump_t *frontier = (nfa_jump_t *)malloc(sizeof(nfa_jump_t));
    *frontier = (nfa_jump_t){0};
    frontier->node = fsa;

    fsa_node_t *restore_to = fsa;
    int32_t parens_depth = 0;
    bool is_escaped = false;
    size_t pattern_len = strlen(pattern);
    nfa_jump_t **tail = &frontier->_next;

    for (int reg_index = 0; reg_index < pattern_len; reg_index++)
    {
        char c = pattern[reg_index];

        if (parens_depth > 0 || (c == ')' && !is_escaped))
        {
            // Within parentheses; discard characters
            switch (c)
            {
            case '(':
                parens_depth++;
                break;

            case ')':
                if (--parens_depth < 0) goto outer_break;
                else break;
            }
            // Discard all characters, including balanced ')'
            continue;
        }

        if (is_escaped)
        {
            goto escaped;
        }
        switch (c)
        {
        case '\\':
            is_escaped = true;
            continue;

        case '|':
            {
                fsa_node_t *sub_expr = (fsa_node_t *)malloc(sizeof(fsa_node_t));
                *sub_expr = (fsa_node_t){0};

                nfa_jump_t *list_entry = (nfa_jump_t *)malloc(sizeof(nfa_jump_t));
                *list_entry = (nfa_jump_t){0};
                list_entry->node = sub_expr;
                list_entry->_next = fsa->eps;
                fsa->eps = list_entry;

                // Append frontier
                *tail = build_nfa(sub_expr, &pattern[reg_index + 1], accept);
                // Move tail forward to end
                for (; *tail; tail = &(*tail)->_next);
            }
            goto outer_break;

        case '(':
            {
                // Enter parentheses discarding mode
                parens_depth++;
                fsa_node_t *sub_expr = (fsa_node_t *)malloc(sizeof(fsa_node_t));
                *sub_expr = (fsa_node_t){0};
                restore_to = sub_expr;

                for (nfa_jump_t *state = frontier; state; state = state->_next)
                {
                    nfa_jump_t *list_entry = (nfa_jump_t *)malloc(sizeof(nfa_jump_t));
                    *list_entry = (nfa_jump_t){0};
                    list_entry->node = sub_expr;
                    list_entry->_next = state->node->eps;
                    state->node->eps = list_entry;
                }

                frontier = build_nfa(sub_expr, &pattern[reg_index + 1], 0);
                tail = &frontier;
                // Move tail forward to end
                for (; *tail; tail = &(*tail)->_next);
            }
            continue;

        case '+':
            for (nfa_jump_t *state = frontier; state; state = state->_next)
            {
                nfa_jump_t *list_entry = (nfa_jump_t *)malloc(sizeof(nfa_jump_t));
                *list_entry = (nfa_jump_t){0};
                list_entry->node = restore_to;
                list_entry->_next = state->node->eps;
                state->node->eps = list_entry;
            }
            continue;
        }
        is_escaped = false;
    escaped:

        fsa_node_t *use_state = (fsa_node_t *)malloc(sizeof(fsa_node_t));
        *use_state = (fsa_node_t){0};
        use_state->letter = c;

        restore_to = (fsa_node_t *)malloc(sizeof(fsa_node_t));
        *restore_to = (fsa_node_t){0};

        restore_to->actions = (dfa_jump_t *)malloc(sizeof(dfa_jump_t));
        *restore_to->actions = (dfa_jump_t){0};
        restore_to->actions->letter = c;
        restore_to->actions->node = use_state;

        for (nfa_jump_t *state = frontier; state; state = state->_next)
        {
            dfa_jump_t *existing = find_dfa_action(state->node, c);
            if (existing)
            {
                nfa_jump_t *list_entry = (nfa_jump_t *)malloc(sizeof(nfa_jump_t));
                *list_entry = (nfa_jump_t){0};
                list_entry->node = restore_to;
                list_entry->_next = state->node->eps;
                state->node->eps = list_entry;
            } else
            {
                dfa_jump_t *list_entry = (dfa_jump_t *)malloc(sizeof(dfa_jump_t));
                *list_entry = (dfa_jump_t){0};
                list_entry->letter = c;
                list_entry->node = use_state;
                list_entry->_next = state->node->actions;
                state->node->actions = list_entry;
            }
        }

        frontier = (nfa_jump_t *)malloc(sizeof(nfa_jump_t));
        *frontier = (nfa_jump_t){0};
        frontier->node = use_state;
        tail = &frontier->_next;
    }
outer_break:

    if (accept > 0)
    {
        for (nfa_jump_t *state = frontier; state; state = state->_next)
        {
            fsa_accepts_t *list_entry = (fsa_accepts_t *)malloc(sizeof(fsa_accepts_t));
            *list_entry = (fsa_accepts_t){0};
            list_entry->accepts = accept;
            list_entry->_next = state->node->accepts;
            state->node->accepts = list_entry;
        }
    }

    return frontier;
}

fsa_node_t *convert_to_dfa(fsa_node_t *fsa)
{
    return NULL;
}