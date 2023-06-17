using Jibini.Polymer.Prototype.Lexer;
using Jibini.Polymer.Prototype.Parser;

namespace Jibini.Polymer.Prototype.Grammar;

using static Token;

public class TypeParameters : NonTerminal<List<TypeDto?>>
{
    override public bool TryMatch(TokenStream source, out List<TypeDto?>? dto)
    {
        _ = MatchSeries(source, Lt);

        dto = new();
        while (Valid && source.Next != Gt)
        {
            var data = MatchSeries(source, new Type());
            dto.Add(data[0] as TypeDto);

            if (source.Next != Gt)
            {
                _ = MatchSeries(source, Comma);
            }
        }

        _ = MatchSeries(source, Gt);
        return Valid;
    }
}