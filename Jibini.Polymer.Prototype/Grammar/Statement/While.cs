using Jibini.Polymer.Prototype.Lexer;
using Jibini.Polymer.Prototype.Parser;

namespace Jibini.Polymer.Prototype.Grammar;

public class WhileDto : StatementDto
{
    public override string _Type => "While";

    public ExpressionDto? Predicate { get; set; }
    public BodyDto Body { get; set; } = new();
}

public class While : NonTerminal<WhileDto>
{
    public override bool TryMatch(TokenStream source, out WhileDto? dto)
    {
        var data = MatchSeries(source,
            Token.While, new Parens(),
            new Body()
            );
        dto = new()
        {
            Predicate = data[1] as ExpressionDto,
            Body = (data[2] as BodyDto)!
        };
        return Valid;
    }
}