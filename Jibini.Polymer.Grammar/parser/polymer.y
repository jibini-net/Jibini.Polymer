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

%type <string_t> _Ident
%type <string_t> Ident
%type <ptr_t> Prog
%type <ptr_t> TopLevels
%type <ptr_t> TopLevel
%type <ptr_t> TypeDecl
%type <ptr_t> Declaration
%type <ptr_t> Function
%type <ptr_t> Vars
%type <ptr_t> Var
%type <ptr_t> Type

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
%token CHAR_LIT
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
Prog            : TopLevels                                 { $$ = tok__Prog__a($1); }
                |                                           { $$ = tok__Prog__b(); }
TopLevels       : TopLevels TopLevel                        { $$ = tok__TopLevels__a($1, $2); }
                | TopLevel                                  { $$ = tok__TopLevels__b($1); }

                /* Valid top-level declaration types */
TopLevel        : TypeDecl                                  { $$ = tok__TopLevel__a($1); }
                | Declaration                               { $$ = tok__TopLevel__b($1); }
                | Function                                  { $$ = tok__TopLevel__c($1); }
                /* Error boundary to prevent process from exiting */
                | error                                     { $$ = NULL; }

                /* Type interface and structure definitions */
TypeDecl        : TYPE Ident SemiC                          { free($2); }
                | INTERFACE Ident SemiC                     { free($2); }

                /* Global and inline variable declarations */
Vars            : Vars ',' Var                              { $$ = tok__Vars__a($1, $3); }
                | Var                                       { $$ = tok__Vars__b($1); }
Var             : Ident Colon Type                          { $$ = tok__Var__a($1, $3); }
                | '*' Ident Colon Type                      { $$ = tok__Var__b($2, $4); }
Declaration     : VAR Vars SemiC                            { }
Type            : Ident TypeParam                           { free($1); }
Types           : Types ',' Type                            { }
                | Type                                      { }
TypeParam       : LT Types Gt                               { }
                |                                           { }

                /* Function signature and body definitions */
Function        : FUN Ident TypeParam
                  OpenP FuncVars CloseP FuncType
                  FuncBody                                  { free($2); }
FuncVars        : FuncVars ',' FuncVar                      { }
                | FuncVar                                   { }
                |                                           { }
FuncVar         : _Ident Colon Type                         { free($1); }
FuncType        : ':' Type                                  { }
                |                                           { }
FuncBody        : Block                                     { }
                | error                                     { write_mesg(stderr, "Expected function body"); }
Block           : '{' CloseB                                { }

                /* Error handling for common expected tokens */
_Ident          : IDENT                                     { $$ = strdup(yytext); }
Ident           : _Ident                                    { $$ = $1; }
                | error                                     { write_mesg(stderr, "Expected identifier"); }
OpenP           : '('                                       { }
                | error                                     { write_mesg(stderr, "Expected '('"); }
CloseP          : ')'                                       { }
                | error                                     { write_mesg(stderr, "Expected ')'"); }
SemiC           : ';'                                       { }
                | error                                     { write_mesg(stderr, "Expected ';'"); }
Colon           : ':'                                       { }
                | error                                     { write_mesg(stderr, "Expected ':'"); }
CloseB          : '}'                                       { }
                | error                                     { write_mesg(stderr, "Expected '}'"); }
Gt              : GT                                        { }
                | error                                     { write_mesg(stderr, "Expected '>'"); }
%%

char *msg = NULL;

int yyerror(char *_) {
    char prefix[] = "Unexpected token '";
    size_t yylen = strlen(yytext);
    size_t new_len = sizeof(prefix) + yylen + 1;
    char *msg = (char *)malloc(new_len);

    snprintf(msg, new_len, "%s%s'", prefix, yytext);
    write_mesg(stderr, msg);

    free(msg);
    return 1;
}
