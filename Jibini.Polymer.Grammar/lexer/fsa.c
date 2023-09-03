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
    // Deref 1: moves end-of-list pointer
    // Deref 2: reassigns "next" on caboose node
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
    return NULL;
}

fsa_node_t convert_to_dfa(fsa_node_t *fsa)
{
    return (fsa_node_t){0};
}