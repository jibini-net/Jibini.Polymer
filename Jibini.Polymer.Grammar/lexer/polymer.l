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
    #include "lexer/lincludes.h"
%}

letter [A-Za-z]
digit [0-9]

%%
var                             { return VAR; }
if                              { return IF; }
else                            { return ELSE; }
while                           { return WHILE; }
for                             { return FOR; }
do                              { return DO; }
select|SELECT                   { return SELECT; }
from|FROM                       { return FROM; }
where|WHERE                     { return WHERE; }
in|IN                           { return IN; }
fun                             { return FUN; }
return                          { return RETURN; }
new                             { return NEW; }
type                            { return TYPE; }
interface                       { return INTERFACE; }
public                          { return PUBLIC; }
private                         { return PRIVATE; }
final                           { return FINAL; }
buffer                          { return BUFFER; }
_*{letter}({letter}|{digit}|_)* { return IDENT; }
{digit}{digit}*                 { return INT_LIT; }
\"(\\.|[^"\\])*\"               { return STR_LIT; }
\<                              { return LT; }
\<\=                            { return LTE; }
\>                              { return GT; }
\>\=                            { return GTE; }
\!\=                            { return N_EQUALS; }
\&\&                            { return AND; }
\|\|                            { return OR; }
\=\=                            { return EQUALS; }
\-\>                            { return PTR; }
\=                              { return '='; }
\+                              { return '+'; }
\*                              { return '*'; }
\;                              { return ';'; }
\(                              { return '('; }
\)                              { return ')'; }
\{                              { return '{'; }
\}                              { return '}'; }
\[                              { return '['; }
\]                              { return ']'; }
\-                              { return '-'; }
\:                              { return ':'; }
\/                              { return '/'; }
\%                              { return '%'; }
\^                              { return '^'; }
\!                              { return '!'; }
\,                              { return ','; }
\&                              { return '&'; }
\?                              { return '?'; }
\'                              { return '\''; }
[ ]                             { }
\t                              { }
\r                              { }
\n                              { }
.                               {
                                    write_message(stderr, "Unexpected character(s) in input");
                                    shutdown();
                                    exit(EXIT_FAILURE);
                                }
%%

int yywrap() {
    return 1;
}