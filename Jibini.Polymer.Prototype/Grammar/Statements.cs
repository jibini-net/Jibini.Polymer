using Jibini.Polymer.Prototype.Lexer;
using Jibini.Polymer.Prototype.Parser;

namespace Jibini.Polymer.Prototype.Grammar;

using static Token;

public abstract class StatementDto
{
    public abstract string Type { get; }
}

public class Statements : NonTerminal<List<StatementDto>>
{
    override public bool TryMatch(TokenStream source, out List<StatementDto>? dto)
    {
        dto = new();
        while ((source.Next ?? RCurly) != RCurly)
        {
            var data = MatchOptions(source,
                new Function(),
                new Body(),
                Semic)
                as StatementDto;
            if (data is not null)
            {
                dto.Add(data);
            }
        }
        return Valid;
    }
}