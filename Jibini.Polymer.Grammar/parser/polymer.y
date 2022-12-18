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

%%
Prog            : Var                                       { }
Var             : VAR                                       { $$ = NULL; }
%%

int yyerror(char *s) {
    return 1;
}