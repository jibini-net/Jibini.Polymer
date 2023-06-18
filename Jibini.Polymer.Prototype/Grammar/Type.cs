using Jibini.Polymer.Prototype.Lexer;
using Jibini.Polymer.Prototype.Parser;

namespace Jibini.Polymer.Prototype.Grammar;

using static Token;

public class TypeDto
{
    public string Name { get; set; } = "";
    public List<TypeDto>? TypeParams { get; set; } = new();
}

public class Type : NonTerminal<TypeDto>
{
    override public bool TryMatch(TokenStream source, out TypeDto? dto)
    {
        if (source.Next != Token.Ident)
        {
            dto = null;
            return Valid = false;
        }
        var data = MatchSeries(source, new Ident());
        dto = new()
        {
            Name = (data[0] as IdentDto)?.Name ?? ""
        };

        if (source.Next == Lt)
        {
            data = MatchSeries(source, new TypeParams());
            dto.TypeParams = (data[0] as List<TypeDto>)!;
        }
        return Valid;
    }
}