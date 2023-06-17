using Jibini.Polymer.Prototype.Lexer;
using Jibini.Polymer.Prototype.Parser;

namespace Jibini.Polymer.Prototype.Grammar;

using static Token;

public class BodyDto
{
}

public class Body : NonTerminal<BodyDto>
{
    public override bool TryMatch(TokenStream source, out BodyDto? dto)
    {
        _ = MatchSeries(source,

            LCurly, RCurly

            );
        dto = new()
        {
        };
        return Valid;
    }
}