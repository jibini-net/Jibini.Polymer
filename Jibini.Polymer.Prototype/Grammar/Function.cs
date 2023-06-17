using Jibini.Polymer.Prototype.Lexer;
using Jibini.Polymer.Prototype.Parser;

namespace Jibini.Polymer.Prototype.Grammar;

using static Token;

public class FunctionDto
{
    public IdentDto? Ident { get; set; }
    public BodyDto? Body { get; set; }
}

public class Function : NonTerminal<FunctionDto>
{
    public override bool TryMatch(TokenStream source, out FunctionDto? dto)
    {
        var data = MatchSeries(source,

            Fun, new Ident(), LParens, /*...,*/ RParens,

            new Body()

            );
        dto = new()
        {
            Ident = data[1] as IdentDto,
            Body = data[4] as BodyDto
        };
        return Valid;
    }
}