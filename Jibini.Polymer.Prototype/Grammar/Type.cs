using Jibini.Polymer.Prototype.Lexer;
using Jibini.Polymer.Prototype.Parser;

namespace Jibini.Polymer.Prototype.Grammar;

using static Token;

public class TypeDto
{
    public IdentDto Name { get; set; } = new();
    public int PointerDepth { get; set; }
}

public class Type : NonTerminal<TypeDto>
{
    override public bool TryMatch(TokenStream source, out TypeDto? dto)
    {
        var data = MatchSeries(source, new Ident());
        dto = new()
        {
            Name = (data[0] as IdentDto)!
        };

        for (; source.Next == Mult; MatchSeries(source, Mult), dto.PointerDepth++);
        return Valid;
    }
}