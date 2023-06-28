using Jibini.Polymer.Prototype.Lexer;
using Jibini.Polymer.Prototype.Parser;

namespace Jibini.Polymer.Prototype.Grammar;

using static Token;

public abstract class ExpressionDto
{
    public abstract string _Type { get; }
}

public class ExprBiOpDto : ExpressionDto
{
    public override string _Type => "Binary";

    public string Op { get; set; } = "";
    public ExpressionDto? Expr1 { get; set; }
    public ExpressionDto? Expr2 { get; set; }
}

public class ExprUnOpDto : ExpressionDto
{
    public override string _Type => "Unary";

    public string Op { get; set; } = "";
    public ExpressionDto? Expr { get; set; }
}

public class ExpressionA : NonTerminal<ExpressionDto>
{
    override public bool TryMatch(TokenStream source, out ExpressionDto? dto)
    {
        dto = MatchOptions(source,
            new Ident(),
            new Parens(),
            new Lambda()
            // , new [...]Literal(), ...
            )
            as ExpressionDto;

        while (Valid)
        {
            var n = source.Next;
            switch (n)
            {
                case Dot:
                    var data = MatchSeries(source,
                        n, new Ident()
                        );
                    dto = new ExprBiOpDto()
                    {
                        Op = n!.Value.ToString(),
                        Expr1 = dto,
                        Expr2 = data[1] as ExpressionDto
                    };
                    break;

                case LParens:
                    data = MatchSeries(source, new FuncCall());
                    (data[0] as FuncCallDto)!.Target = dto;
                    dto = data[0] as FuncCallDto;
                    break;

                default:
                    goto outer_break;
            }
        }
    outer_break:
        return Valid;
    }
}

public class ExpressionB : NonTerminal<ExpressionDto>
{
    override public bool TryMatch(TokenStream source, out ExpressionDto? dto)
    {
        var negate = source.Next == Not;
        if (negate)
        {
            _ = MatchSeries(source, Not);
        }

        var data = MatchSeries(source, new ExpressionA());
        dto = data[0] as ExpressionDto;

        while (Valid)
        {
            var n = source.Next;
            switch (n)
            {
                case Caret:
                    data = MatchSeries(source, new Exponent());
                    (data[0] as ExprBiOpDto)!.Expr1 = dto;
                    dto = data[0] as ExprBiOpDto;
                    break;

                default:
                    goto outer_break;
            }
        }
    outer_break:

        if (negate)
        {
            dto = new ExprUnOpDto()
            {
                Op = "Negate",
                Expr = dto
            };
        }
        return Valid;
    }
}

public class ExpressionC : NonTerminal<ExpressionDto>
{
    override public bool TryMatch(TokenStream source, out ExpressionDto? dto)
    {
        var data = MatchSeries(source, new ExpressionB());
        dto = data[0] as ExpressionDto;

        while (Valid)
        {
            var n = source.Next;
            switch (n)
            {
                case Mult:
                case Div:
                case Mod:
                    data = MatchSeries(source,
                        n, new ExpressionB()
                        );
                    dto = new ExprBiOpDto()
                    {
                        Op = n!.Value.ToString(),
                        Expr1 = dto,
                        Expr2 = data[1] as ExpressionDto
                    };
                    break;

                default:
                    goto outer_break;
            }
        }
    outer_break:
        return Valid;
    }
}

public class ExpressionD : NonTerminal<ExpressionDto>
{
    override public bool TryMatch(TokenStream source, out ExpressionDto? dto)
    {
        var data = MatchSeries(source, new ExpressionC());
        dto = data[0] as ExpressionDto;

        while (Valid)
        {
            var n = source.Next;
            switch (n)
            {
                case Add:
                case Sub:
                    data = MatchSeries(source,
                        n, new ExpressionC()
                        );
                    dto = new ExprBiOpDto()
                    {
                        Op = n!.Value.ToString(),
                        Expr1 = dto,
                        Expr2 = data[1] as ExpressionDto
                    };
                    break;

                default:
                    goto outer_break;
            }
        }
    outer_break:
        return Valid;
    }
}

public class Expression : NonTerminal<ExpressionDto>
{
    override public bool TryMatch(TokenStream source, out ExpressionDto? dto)
    {
        dto = MatchOptions(source,
            new Assignment(),
            // new [...](), ...,
            new ExpressionD())
            as ExpressionDto;
        return Valid;
    }
}

public class ExprStatementDto : StatementDto
{
    public override string _Type => "Expr";

    public ExpressionDto? Expr { get; set; }
}

public class ExprStatement : NonTerminal<ExprStatementDto>
{
    override public bool TryMatch(TokenStream source, out ExprStatementDto? dto)
    {
        var data = MatchSeries(source,
            new Expression(), Semic
            );
        dto = new()
        {
            Expr = data[0] as ExpressionDto
        };
        return Valid;
    }
}