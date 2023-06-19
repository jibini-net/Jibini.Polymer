using Jibini.Polymer.Prototype.Lexer;
using Jibini.Polymer.Prototype.Parser;

namespace Jibini.Polymer.Prototype.Grammar;

using static Token;

public class Parens : NonTerminal<ExpressionDto>
{
    override public bool TryMatch(TokenStream source, out ExpressionDto? dto)
    {
        if (source.Next != LParens)
        {
            dto = null;
            return Valid = false;
        }
        var data = MatchSeries(source,
            LParens, new Expression(), RParens
            );
        dto = data[1] as ExpressionDto;
        return Valid;
    }
}