using Jibini.Polymer.Prototype.Lexer;
using Jibini.Polymer.Prototype.Parser;

namespace Jibini.Polymer.Prototype.Grammar;

using static Token;

public class BodyDto : StatementDto
{
    override public string Type => "Body";

    public List<StatementDto>? Statements { get; set; }
}

public class Body : NonTerminal<BodyDto>
{
    override public bool TryMatch(TokenStream source, out BodyDto? dto)
    {
        var data = MatchSeries(source,
            LCurly,
                new Statements(endToken: RCurly),
            RCurly
            );
        dto = new()
        {
            Statements = data[1] as List<StatementDto>
        };
        return Valid;
    }
}