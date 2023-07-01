using Jibini.Polymer.Prototype.Lexer;
using Jibini.Polymer.Prototype.Parser;

namespace Jibini.Polymer.Prototype.Grammar;

using static Token;

public class IfElseDto : StatementDto
{
    public override string _Type => "IfElse";

    public ExpressionDto? Predicate { get; set; }
    public BodyDto Body { get; set; } = new();
    public BodyDto? ElseBody { get; set; }
}

public class IfElse : NonTerminal<IfElseDto>
{
    public override bool TryMatch(TokenStream source, out IfElseDto? dto)
    {
        var data = MatchSeries(source,
            If, new Parens(),
            new Body()
            );
        dto = new()
        {
            Predicate = data[1] as ExpressionDto,
            Body = (data[2] as BodyDto)!
        };

        if (source.Next == Else)
        {
            data = MatchSeries(source,
                Else,
                new Body()
                );
            dto.ElseBody = data[1] as BodyDto;
        }
        return Valid;
    }
}