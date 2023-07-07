using Jibini.Polymer.Prototype.Grammar;

namespace Jibini.Polymer.Prototype.Interpreter;

using Expr = System.Linq.Expressions;

public class StatementImpl
{
    public static Action<State> Meaning(StatementDto statement)
    {
        switch (statement._Type)
        {
            case "Function":
                {
                    var dto = (statement as FunctionDto)!;
                    var meaning = Meaning(dto.Body);
                    return (state) =>
                    {
                        state = new State(state);
                        var variable = new Variable()
                        {
                            Name = dto.Name,
                            Type = new() { Name = new() { Name = "Fun" } },
                            Value = (State state) =>
                            {
                                meaning(state);
                            }
                        };
                    };
                }

            case "Declaration":
                {
                    var dto = (statement as DeclarationDto)!;
                    var variable = new Variable()
                    {
                        Name = dto.Name,
                        Type = dto.Type ?? new() // TODO Inferred types
                    };
                    var init = ExpressionImpl.Meaning(dto.InitialValue);
                    return (state) =>
                    {
                        state.Declare(variable);
                        variable.Value = init(state);
                    };
                }

            case "IfElse":
                {
                    var dto = (statement as IfElseDto)!;
                    var test = ExpressionImpl.Meaning(dto.Predicate);
                    var success = Meaning(dto.Body);
                    var failure = dto.ElseBody is null ? null : Meaning(dto.ElseBody);
                    return (state) =>
                    {
                        if (test(state) as bool? == true)
                        {
                            success(state);
                        } else
                        {
                            failure?.Invoke(state);
                        }
                    };
                }

            case "While":
                {
                    var dto = (statement as WhileDto)!;
                    var test = ExpressionImpl.Meaning(dto.Predicate);
                    var success = Meaning(dto.Body);
                    return (state) =>
                    {
                        while (test(state) as bool? == true)
                        {
                            success(state);
                        }
                    };
                }

            case "For":
                {
                    var dto = (statement as ForDto)!;
                    var decl = Meaning(dto.Declaration);
                    var test = ExpressionImpl.Meaning(dto.Predicate);
                    var adv = ExpressionImpl.Meaning(dto.Advancement);
                    var success = Meaning(dto.Body);
                    return (state) =>
                    {
                        state = new State(state);
                        for (decl(state);
                            test(state) as bool? == true;
                            adv(state))
                        {
                            success(state);
                        }
                    };
                }

            case "ForEach":
                {
                    var dto = (statement as ForEachDto)!;
                    var decl = Meaning(dto.Declaration);
                    var items = ExpressionImpl.Meaning(dto.Items);
                    var success = Meaning(dto.Body);
                    return (state) =>
                    {
                        decl(state);
                        var its = items(state) as IEnumerable<object?> ?? Array.Empty<object?>();
                        foreach (var it in its)
                        {
                            success(state);
                        }
                    };
                }

            case "Body":
                {
                    var dto = (statement as BodyDto)!;
                    var meaning = Meaning(dto.Statements);
                    return (state) =>
                    {
                        state = new State(state);
                        meaning(state);
                    };
                }

            case "Expr":
                {
                    var dto = (statement as ExprStatementDto)!;
                    var meaning = ExpressionImpl.Meaning(dto.Expr);
                    return (state) => 
                    {
                        _ = meaning(state);
                    };
                }

            default:
                throw new Exception($"Unexpected statement of type '{statement._Type}'");
        }
    }

    public static Action<State> Meaning(List<StatementDto> statements)
    {
        var meanings = statements.Select(Meaning).ToList();
        return (state) =>
        {
            foreach (var it in meanings)
            {
                it(state);
            }
        };
    }
}
