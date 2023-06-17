namespace Jibini.Polymer;

class Terminal<T> : NonTerminal<T> where T : class
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
        //
        // Constructor controls the stream until the non-terminal is consumed.
        var factory = typeof(T).GetConstructor(new[] { typeof(TokenStream) });
        dto = factory?.Invoke(new[] { source }) as T;
        // Can fall back to the no-argument constructor where state not needed
        if (dto is null)
        {
            factory = typeof(T).GetConstructor(Array.Empty<Type>());
            dto = factory?.Invoke(Array.Empty<object?>()) as T;
        }
        source.Poll();
        return true;
    }
}