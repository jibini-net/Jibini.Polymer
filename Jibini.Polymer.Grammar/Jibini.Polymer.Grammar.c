// Copyright (c) 2022-2023 Zach Goethel
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

// Test tokens
#define _DISCARD 9999
#define _SCHEMA 1
#define _PARTIAL 2
#define _REPO 3
#define _SERVICE 4
#define _JSON 5
#define _IDENT 6
#define _LCURLY 7
#define _RCURLY 8
#define _LPAREN 9
#define _RPAREN 10
#define _COMMA 11
#define _SPLAT 12
#define _ASSIGN 13
#define _ARROW 14

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
    fsa_node_t test = {0};

    char word[240] = {0};
    char *letters = "a|b|c|d|e|f|g|h|i|j|k|l|m|n|o|p|q|r|s|t|u|v|w|x|y|z";
    char *cap_letters = "A|B|C|D|E|F|G|H|I|J|K|L|M|N|O|P|Q|R|S|T|U|V|W|X|Y|Z";
    char *numbers = "0|1|2|3|4|5|6|7|8|9";
    snprintf(word, sizeof(word), "(%s|%s|_)+(|(%s|%s|%s|_)+)",
        letters, cap_letters,
        letters, cap_letters, numbers);
    
    build_nfa(&test, "schema",        _SCHEMA);
    build_nfa(&test, "partial",       _PARTIAL);
    build_nfa(&test, "repo",          _REPO);
    build_nfa(&test, "service",       _SERVICE);
    build_nfa(&test, "json",          _JSON);
    build_nfa(&test, word,            _IDENT);
    build_nfa(&test, "\\{",           _LCURLY);
    build_nfa(&test, "\\}",           _RCURLY);
    build_nfa(&test, "\\(",           _LPAREN);
    build_nfa(&test, "\\)",           _RPAREN);
    build_nfa(&test, "\\,",           _COMMA);
    build_nfa(&test, "\\.\\.\\.",     _SPLAT);
    build_nfa(&test, "\\=",           _ASSIGN);
    build_nfa(&test, "\\=\\>",        _ARROW);
    build_nfa(&test, "( |\n|\r|\t)+", _DISCARD);

    //test = *(convert_to_dfa(&test));
}

void shutdown()
{
    free_file();
    yylex_destroy();
}
