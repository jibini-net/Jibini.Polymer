using Jibini.Polymer.Prototype.Lexer;
using Jibini.Polymer.Prototype.Parser;

namespace Jibini.Polymer.Prototype.Grammar;

using static Token;

public class ParameterDto
{
    public IdentDto? Ident { get; set; }
    public TypeDto? Type { get; set; }
}

public class Parameters : NonTerminal<List<ParameterDto>>
{
    override public bool TryMatch(TokenStream source, out List<ParameterDto>? dto)
    {
        _ = MatchSeries(source, LParens);

        dto = new();
        while (Valid && source.Next != RParens)
        {
            var data = MatchSeries(source,

                new Ident(), Colon, new Type()

            );
            dto.Add(new()
            {
                Ident = data[0] as IdentDto,
                Type = data[2] as TypeDto
            });

            if (source.Next != RParens)
            {
                _ = MatchSeries(source, Comma);
            }
        }

        _ = MatchSeries(source, RParens);
        return Valid;
    }
}