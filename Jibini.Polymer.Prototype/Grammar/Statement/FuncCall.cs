using Jibini.Polymer.Prototype.Lexer;
using Jibini.Polymer.Prototype.Parser;
using System.Linq.Expressions;

namespace Jibini.Polymer.Prototype.Grammar;

using static Token;

public class FuncCallDto : ExpressionDto
{
    public override string _Type => "FuncCall";

    public ExpressionDto? Target { get; set; }
    public List<ExpressionDto> Args { get; set; } = new();
}

public class FuncCall : NonTerminal<FuncCallDto>
{
    override public bool TryMatch(TokenStream source, out FuncCallDto? dto)
    {
        var data = MatchSeries(source,
            new ExpressionA(), LParens
            );
        dto = new()
        {
            Target = data[0] as ExpressionDto
        };

        while (Valid && source.Next != RParens)
        {
            var expr = MatchOptions(source, new Expression());
            if (expr is not null)
            {
                dto.Args.Add((expr as ExpressionDto)!);
            }

            if (source.Next != RParens)
            {
                _ = MatchSeries(source, Comma);
            }
        }

        _ = MatchSeries(source, RParens);
        return Valid;
    }
}