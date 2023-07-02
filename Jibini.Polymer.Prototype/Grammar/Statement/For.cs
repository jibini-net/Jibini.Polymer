using Jibini.Polymer.Prototype.Lexer;
using Jibini.Polymer.Prototype.Parser;

namespace Jibini.Polymer.Prototype.Grammar;

using static Token;

public class ForDto : StatementDto
{
    public override string _Type => "For";

    public DeclarationDto? Declaration { get; set; }
    public ExpressionDto? Predicate { get; set; }
    public ExpressionDto? Advancement { get; set; }
    public BodyDto Body { get; set; } = new();
}

public class For : NonTerminal<ForDto>
{
    public override bool TryMatch(TokenStream source, out ForDto? dto)
    {
        IList<object?> data;
        dto = new();
        if (source.Next != Token.For)
        {
            return Valid = false;
        }
        _ = MatchSeries(source,
            Token.For, LParens
            );

        dto.Declaration = MatchOptions(source,
            new Declaration(),
            Semic)
            as DeclarationDto;

        dto.Predicate = (MatchOptions(source,
            new ExprStatement(),
            Semic)
            as ExprStatementDto)?.Expr;

        if (source.Next != RParens)
        {
            data = MatchSeries(source, new Expression());
            dto.Advancement = data[0] as ExpressionDto;
        }

        data = MatchSeries(source,
            RParens,
            new Body()
            );
        dto.Body = (data[1] as BodyDto)!;
        return Valid;
    }
}