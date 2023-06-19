using Jibini.Polymer.Prototype.Lexer;
using Jibini.Polymer.Prototype.Parser;

namespace Jibini.Polymer.Prototype.Grammar;

using static Token;

public class AssignmentDto : ExpressionDto
{
    override public string _Type => "Assignment";

    public ExpressionDto? Target { get; set; }
    public ExpressionDto? Value { get; set; }
}

public class Assignment : NonTerminal<AssignmentDto>
{
    override public bool TryMatch(TokenStream source, out AssignmentDto? dto)
    {
        var data = MatchSeries(source,
            new ExpressionA(), Equal
            );
        dto = new()
        {
            Target = data[0] as ExpressionDto
        };

        if (Valid)
        {
            data = MatchSeries(source, new Expression());
            dto.Value = data[0] as ExpressionDto;
        }
        return Valid;
    }
}