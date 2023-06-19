using Jibini.Polymer.Prototype.Lexer;
using Jibini.Polymer.Prototype.Parser;

namespace Jibini.Polymer.Prototype.Grammar;

using static Token;

public class AssignmentDto : ExpressionDto
{
    override public string _Type => "Assignment";

    public string Name { get; set; } = "";
    public ExpressionDto? Value { get; set; }
}

public class Assignment : NonTerminal<AssignmentDto>
{
    override public bool TryMatch(TokenStream source, out AssignmentDto? dto)
    {
        var data = MatchSeries(source,
            new Ident(), Equal
            );
        dto = new()
        {
            Name = (data[0] as IdentDto)?.Name ?? ""
        };

        if (Valid)
        {
            data = MatchSeries(source, new Expression());
            dto.Value = data[0] as ExpressionDto;
        }
        return Valid;
    }
}