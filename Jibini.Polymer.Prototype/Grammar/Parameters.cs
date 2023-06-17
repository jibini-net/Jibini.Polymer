using Jibini.Polymer.Prototype.Lexer;
using Jibini.Polymer.Prototype.Parser;

namespace Jibini.Polymer.Prototype.Grammar;

using static Token;

public class ParameterDto
{
    public IdentDto? Ident { get; set; }
    public IdentDto? Type { get; set; }
}

public class Parameters : NonTerminal<List<ParameterDto>>
{
    public override bool TryMatch(TokenStream source, out List<ParameterDto>? dto)
    {
        dto = new();
        while (source.Next != RParens)
        {
            var data = MatchSeries(source,

                new Ident(), Colon, new Ident()

            );
            dto.Add(new()
            {
                Ident = data[0] as IdentDto,
                Type = data[2] as IdentDto
            });
            if (source.Next != RParens)
            {
                _ = MatchSeries(source, Comma);
            }
        }
        return Valid;
    }
}