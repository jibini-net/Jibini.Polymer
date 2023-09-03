// Copyright (c) 2022 Zach Goethel
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

#include "Jibini.Polymer.Grammar.h"

#include <stdlib.h>
#include <stdio.h>
#include <fcntl.h>
#include <stdbool.h>

#include "lexer/fsa.h"
#include "lexer/lexer.h"
#include "parser/parser.h"

#include "io/source.h"

int main(int arg_c, char **arg_v)
{
    /*
    // Expects to operate only on standard in/out/error files
    if (arg_c != 1)
    {
        fprintf(stderr, "Usage: %s\n", arg_v[0]);
        return 1;
    }

    // Parse tokens until unrecoverable error or EOF
    begin_file(stdin);
    int result = yyparse();
    // Cleanup state and release copies of input in memory
    shutdown();
    return result;
    */
    fsa_node_t test1 = {0};
    fsa_node_t test2 = {0};
    fsa_node_t test3 = {0};

    fsa_node_t test = {0};
    test.actions = (dfa_jump_t *)malloc(sizeof(dfa_jump_t));
    *test.actions = (dfa_jump_t){0};
    test.actions->letter = 'a';
    test.actions->node = &test2;

    nfa_jump_t _chain = {0};
    test.eps = &_chain;
    nfa_jump_t *chain = &_chain;

    chain->node = &test1;

    chain = (chain->_next) = (nfa_jump_t *)malloc(sizeof(nfa_jump_t));
    *chain = (nfa_jump_t){0};

    chain->node = &test2;

    chain = (chain->_next) = (nfa_jump_t *)malloc(sizeof(nfa_jump_t));
    *chain = (nfa_jump_t){0};

    chain->node = &test3;

    eps_closure_t test_eps = epsilon_closure(&test);
    eps_closure_t test_eps_a = epsilon_closure_on(&test, 'a');
}

void shutdown()
{
    free_file();
    yylex_destroy();
}
