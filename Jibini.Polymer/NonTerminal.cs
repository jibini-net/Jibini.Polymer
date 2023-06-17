namespace Jibini.Polymer;

public abstract class NonTerminal
{
    // Allows terminals to be placed inline with non-terminals in a collection
    public static implicit operator NonTerminal(Token token) => new Terminal<object>(token);

    public abstract bool TryMatch(TokenStream source, out object? dto);

    public bool Valid { get; protected set; } = true;

    protected IEnumerable<object?> MatchSeries(TokenStream source, params NonTerminal[] series)
    {
        foreach (var nonTerm in series)
        {
            Valid &= nonTerm.TryMatch(source, out var dto);
            yield return dto;
        }
    }
}

public abstract class NonTerminal<T> : NonTerminal where T : class
{
    public override bool TryMatch(TokenStream source, out object? dto)
    {
        var result = TryMatch(source, out T? _dto);
        dto = _dto;
        return result;
    }
    public abstract bool TryMatch(TokenStream source, out T? dto);
}