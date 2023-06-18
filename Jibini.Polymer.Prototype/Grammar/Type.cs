using Jibini.Polymer.Prototype.Lexer;
using Jibini.Polymer.Prototype.Parser;

namespace Jibini.Polymer.Prototype.Grammar;

using static Token;

public class TypeDto
{
    public string? Name { get; set; }
    public List<TypeDto>? TypeParams { get; set; }
}

public class Type : NonTerminal<TypeDto>
{
    override public bool TryMatch(TokenStream source, out TypeDto? dto)
    {
        var data = MatchSeries(source, new Ident());
        dto = new()
        {
            Name = (data[0] as IdentDto)?.Name
        };

        dto.TypeParams = MatchOptions(source,
            new TypeParameters(),
            Epsilon)
            as List<TypeDto>;
        return Valid;
    }
}