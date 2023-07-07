using Jibini.Polymer.Prototype.Grammar;

namespace Jibini.Polymer.Prototype.Interpreter;

using Expr = System.Linq.Expressions;

public class ExpressionImpl
{
    public static Func<State, object?> Meaning(ExpressionDto? expression)
    {
        // FuncCall should declare return variable in intermediate name scope
        if (expression is null)
        {
            return (_) => null;
        }
        switch (expression._Type)
        {
            default:
                throw new Exception($"Unexpected expression of type '{expression._Type}'");
        }
    }
}
