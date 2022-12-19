/*
 * Copyright (c) 2022 Zach Goethel
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is furnished
 * to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in 
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
 * IN THE SOFTWARE.
 */

%{
    #include "parser/pincludes.h"
%}

%union {
    int int_t;
    unsigned int uint_t;
    long long_t;
    unsigned long ulong_t;
    char *string_t;
    void *ptr_t;
    int bool_t;
}

%type <ptr_t> Var

%token VAR
%token IF
%token ELSE
%token WHILE
%token FOR
%token DO
%token SELECT
%token FROM
%token WHERE
%token IN
%token FUN
%token RETURN
%token IDENT
%token INT_LIT
%token STR_LIT
%token LT
%token LTE
%token GT
%token GTE
%token N_EQUALS
%token AND
%token OR
%token EQUALS
%token NEW
%token PTR
%token TYPE
%token INTERFACE
%token PUBLIC
%token PRIVATE
%token FINAL
%token BUFFER

%%
Prog            : Var                                       { fprintf(stdout, "Accept\n"); }
                |                                           { fprintf(stdout, "Blank\n"); }
Var             : Var VAR                                   { printf("Node\n");$$ = NULL; }
                | VAR                                       { printf("Leaf\n");$$ = NULL; }
%%

int yyerror(char *error) {
    write_message(stderr, error);
    return 1;
}
