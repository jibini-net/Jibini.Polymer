using Jibini.Polymer.Prototype.Grammar;
using System.Collections.Concurrent;

namespace Jibini.Polymer.Prototype.Interpreter;

public class State
{
    private readonly ConcurrentDictionary<IdentDto, Variable> names = new();
    private readonly ConcurrentBag<IdentDto> newNames = new();

    public State()
    {
    }

    public State(State previous)
    {
        names = new(previous.names);
    }

    public void Declare(Variable variable)
    {
        if (newNames.Contains(variable.Name))
        {
            throw new Exception($"Duplicate identifier '{variable.Name.Name}' with {variable.Name.TypeParams?.Count ?? 0} type params");
        }
        newNames.Add(variable.Name);
        names[variable.Name] = variable;
    }

    public Variable Resolve(IdentDto name)
    {
        if (!names.TryGetValue(name, out var result))
        {
            throw new Exception($"No name '{name.Name}' with {name.TypeParams?.Count ?? 0} type params");
        }
        return result;
    }
}

public class Variable
{
    public IdentDto Name { get; set; } = new();
    public TypeDto Type { get; set; } = new();
    public object? Value { get; set; }
}