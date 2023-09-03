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

#pragma once

#include <stdint.h>
#include <stdio.h>

typedef struct fsa_node fsa_node_t;

// Input source to the tokenizer, which keeps a copy of source which has been
// loaded and the current cursor position within that source
typedef struct token_stream
{
    // Start of the loaded input source, which may be partially unread
    char *source;
    // Character position of next letter to consider
    char *cursor;
    // Remaining source content accessible via buffered file IO
    FILE *_source;
} token_stream_t;

// A single deterministic transition within the finite state automaton
typedef struct dfa_jump
{
    // Letter which will cause the transition to occur
    char letter;
    // Node whose state results after the transition
    fsa_node_t *node;
    // Next jump entry in the node's jump action linked list
    struct dfa_jump *_next;
} dfa_jump_t;

// A single nondeterministic transition within the finite state automaton
typedef struct nfa_jump
{
    // Node whose state results after the transition
    fsa_node_t *node;
    // Next jump entry in the node's epsilon transition linked list
    struct nfa_jump *_next;
} nfa_jump_t;

// Epsilon closure obtained after taking a single letter transition, allowing
// nodes to be linked to form an epsilon closure cache
typedef struct eps_closure
{
    // Letter which will cause all transitions to occur
    char letter;
    // Linked list of all transitions achievable via the single letter
    nfa_jump_t *closure;
    // Next cached epsilon closure in a node's linked closure cache
    struct eps_closure *_next;
} eps_closure_t;

// Single token in a list of tokens accepted by the parent state
typedef struct fsa_accepts
{
    // Token accepted once this state is reached, whose numerical value is also
    // used as a precedence ordering (lower value is higher precedence)
    int32_t accepts;
    // Next acceptance entry in the node's accepted token linked list
    struct fsa_accepts *_next;
} fsa_accepts_t;

// State in a finite state automaton, which represents implicit data about
// previously matched character sequences and legal next character inputs, as
// well as any tokens which should be matched by reaching this state
typedef struct fsa_node
{
    // Letter which was used to arrive in this state; "last read"
    char letter;
    // Linked list of token accepted in this state, where highest precedence is
    // determined by lowest numerical value
    fsa_accepts_t *accepts;
    // Deterministic actions taken for each letter
    dfa_jump_t *actions;
    // Nondeterministic actions taken without consuming letters
    nfa_jump_t *eps;
    // Cached epsilon closures to avoid excessive duplicate calculations; clear
    // this list after completing an NFA to DFA conversion to allow changes to
    // the originating NFA
    eps_closure_t *_eps_cache;
} fsa_node_t;

// Invoked once for each nondeterministic transition found while iterating
// through possible epsilon transitions
typedef void (*nfa_action_it_fun)(fsa_node_t *, void *);

// Selects a deterministic action for the provided letter, if available
dfa_jump_t *find_dfa_action(fsa_node_t *fsa, char letter);

// Iterates over available epsilon transitions, allowing execution of actions
void exec_nfa_actions(fsa_node_t *fsa, nfa_action_it_fun it_fun, void *it_fun_args);

// Calculates all states reachable without consuming any letters
eps_closure_t epsilon_closure(fsa_node_t *fsa);

// Calculates all states reachable by consuming the single provided letter
eps_closure_t epsilon_closure_on(fsa_node_t *fsa, char letter);

// Creates new nodes in the automaton to match the provided word to the correct
// token, likely resulting in a nondeterministic set of states
//
// Returns a set of all leaf nodes produced at the end of the built chain
nfa_jump_t *build_nfa(fsa_node_t *fsa, char *pattern, int32_t accept);

// Performs an expensive conversion from NFA to DFA, which ensures each distinct
// node has at most one possible transition per character input
fsa_node_t *convert_to_dfa(fsa_node_t *fsa);