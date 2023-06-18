using Jibini.Polymer.Prototype.Lexer;
using Jibini.Polymer.Prototype.Parser;

namespace Jibini.Polymer.Prototype.Grammar;

using static Token;

public class BodyDto : StatementDto
{
    override public string _Type => "Body";

    public List<StatementDto> Statements { get; set; } = new();
}

public class Body : NonTerminal<BodyDto>
{
    override public bool TryMatch(TokenStream source, out BodyDto? dto)
    {
        if (source.Next != LCurly)
        {
            dto = null;
            return Valid = false;
        }
        var data = MatchSeries(source,
            LCurly,
                new Statements(endToken: RCurly),
            RCurly
            );
        dto = new()
        {
            Statements = (data[1] as List<StatementDto>)!
        };
        return Valid;
    }
}