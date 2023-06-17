namespace Jibini.Polymer;

/// <summary>
/// Allows a range of characters to be mapped to a leaf node within an abstract
/// syntax tree. Creates an object containing names, types, and other details.
/// </summary>
/// <typeparam name="T">DTO class which will be instantiated with details.</typeparam>
public class Terminal<T> : NonTerminal<T> where T : class
{
    private readonly Token terminal;

    public Terminal(Token terminal)
    {
        this.terminal = terminal;
    }

    public override bool TryMatch(TokenStream source, out T? dto)
    {
        if (source.Next != terminal)
        {
            dto = default;
            return false;
        }

        // Output parameter will be populated if there is a constructor on the
        // DTO type accepting a single parameter which is a stream of tokens.
        var factory = typeof(T).GetConstructor(new[] { typeof(TokenStream) });
        dto = factory?.Invoke(new[] { source }) as T;
        // Can fall back to zero-argument constructor where state not needed
        if (dto is null)
        {
            factory = typeof(T).GetConstructor(Array.Empty<Type>());
            dto = factory?.Invoke(Array.Empty<object?>()) as T;
        }
        source.Poll();
        return true;
    }
}