#pragma once

#include <string.h>
#include <stdlib.h>
#include <stdio.h>
#include <stdbool.h>

extern int yylex();
extern char *yytext;
extern int yyerror(char *s);