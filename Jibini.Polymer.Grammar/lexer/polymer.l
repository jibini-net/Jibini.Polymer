%{
    #include "lexer/lincludes.h"
%}

letter [A-Za-z]
digit [0-9]

%%
var                         { return VAR; }
[ ]                         {  }
\t                          {  }
\r                          {  }
\n                          {  }
.                           {

                            }
%%

int yywrap () {
    return 1;
}