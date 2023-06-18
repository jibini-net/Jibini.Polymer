using Jibini.Polymer.Prototype.Lexer;
using Jibini.Polymer.Prototype.Parser;

namespace Jibini.Polymer.Prototype.Grammar;

using static Token;

public abstract class ExpressionDto
{
    public abstract string _Type { get; }
}

public abstract class UnaryExpressionDto : ExpressionDto
{
    public ExpressionDto? Expr { get; set; }
}

public abstract class BiExpressionDto : ExpressionDto
{
    public ExpressionDto? Left { get; set; }
    public ExpressionDto? Right { get; set; }
}

public class Expression : NonTerminal<ExpressionDto>
{
    private readonly Token? endToken;

    public Expression(Token? endToken)
    {
        this.endToken = endToken;
    }

    override public bool TryMatch(TokenStream source, out ExpressionDto? dto)
    {
        dto = MatchOptions(source,
            new Ident())
            as ExpressionDto;
        return Valid;
    }
}