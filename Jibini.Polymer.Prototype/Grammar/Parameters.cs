using Jibini.Polymer.Prototype.Lexer;
using Jibini.Polymer.Prototype.Parser;

namespace Jibini.Polymer.Prototype.Grammar;

using static Token;

public class ParameterDto
{
    public IdentDto Name { get; set; } = new();
    public TypeDto Type { get; set; } = new();
}

public class Parameters : NonTerminal<List<ParameterDto>>
{
    override public bool TryMatch(TokenStream source, out List<ParameterDto>? dto)
    {
        dto = new();
        _ = MatchSeries(source, LParens);

        while (Valid && source.Next != RParens)
        {
            var data = MatchSeries(source,
                new Ident(), Colon, new Type()
                );
            dto.Add(new()
            {
                Name = (data[0] as IdentDto)!,
                Type = (data[2] as TypeDto)!
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