﻿using Jibini.Polymer.Prototype.Lexer;
using Jibini.Polymer.Prototype.Parser;

namespace Jibini.Polymer.Prototype.Grammar;

using static Token;

public class Exponent : NonTerminal<ExprBiOpDto>
{
    override public bool TryMatch(TokenStream source, out ExprBiOpDto? dto)
    {
        _ = MatchSeries(source, Caret);
        dto = new()
        {
            Op = Caret.ToString()
        };

        if (Valid)
        {
            var data = MatchSeries(source, new ExpressionB());
            dto.Expr2 = data[0] as ExpressionDto;
        }
        return Valid;
    }
}