using Jibini.Polymer.Prototype.Lexer;
using Jibini.Polymer.Prototype.Parser;

namespace Jibini.Polymer.Prototype.Grammar;

using static Token;

public abstract class ExpressionDto
{
    public abstract string _Type { get; }
}

public class ExpressionA : NonTerminal<ExpressionDto>
{
    override public bool TryMatch(TokenStream source, out ExpressionDto? dto)
    {
        dto = MatchOptions(source,
            new Ident())
            as ExpressionDto;
        return Valid;
    }
}

public class ExpressionB : NonTerminal<ExpressionDto>
{
    override public bool TryMatch(TokenStream source, out ExpressionDto? dto)
    {
        dto = MatchOptions(source,
            new FuncCall(),
            new ExpressionA())
            as ExpressionDto;
        return Valid;
    }
}

public class Expression : NonTerminal<ExpressionDto>
{
    override public bool TryMatch(TokenStream source, out ExpressionDto? dto)
    {
        dto = MatchOptions(source,
            new Assignment(),
            new ExpressionB())
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