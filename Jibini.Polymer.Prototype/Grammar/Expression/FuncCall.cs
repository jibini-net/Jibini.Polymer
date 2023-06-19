using Jibini.Polymer.Prototype.Lexer;
using Jibini.Polymer.Prototype.Parser;

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
        _ = MatchSeries(source, LParens);

        dto = new();
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