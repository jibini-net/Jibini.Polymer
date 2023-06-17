using Jibini.Polymer.Prototype.Lexer;

namespace Jibini.Polymer.Prototype.Parser;

/// <summary>
/// Sequence of source tokens which is composed of other non-terminals, or
/// a mixed sequence of terminals and non-terminals.
/// 
/// Any instance of this type is only intended to attempt one match.
/// </summary>
public abstract class NonTerminal
{
    // Allows tokens to be placed inline with non-terminals in a collection
    public static implicit operator NonTerminal(Token token) =>
        new Terminal<object?>(token);

    /// <summary>
    /// Wraps a token such that it will be parsed out into the DTO collection
    /// with a certain type, using the DTO type's constructor.
    /// </summary>
    /// <typeparam name="T">DTO type which will be instantiated during parsing.</typeparam>
    /// <param name="token">Stream from which tokens are consumed.</param>
    /// <returns>A trivial non-terminal wrapper for the terminal token.</returns>
    public static Terminal<T> To<T>(Token token) where T : class? =>
        new(token);

    /// <summary>
    /// Attempts to parse the provided source stream as this type of non-
    /// terminal.
    /// </summary>
    /// <param name="source">Stream from which tokens are consumed.</param>
    /// <param name="dto">Object in which text literal details can be placed.</param>
    /// <returns>Whether the syntax was matched in a valid fashion.</returns>
    public abstract bool TryMatch(TokenStream source, out object? dto);

    /// <summary>
    /// Whether the syntax is valid for this element in the source. Initially
    /// set to valid; any unsuccessful attempt to match will unset.
    /// </summary>
    public bool Valid { get; protected set; } = true;

    private IEnumerable<object?> _MatchSeries(TokenStream source, params NonTerminal[] series)
    {
        foreach (var nonTerm in series)
        {
            Valid &= nonTerm.TryMatch(source, out var dto);
            yield return dto;
        }
    }

    /// <summary>
    /// Checks an ordered set of non-terminals, expecting each one in contiguous
    /// order one after the other.
    /// </summary>
    /// <param name="source">Stream from which tokens are consumed.</param>
    /// <param name="series">Ordered series of constituent non-terminals.</param>
    /// <returns>Ordered set of DTOs corresponding to each member.</returns>
    protected IList<object?> MatchSeries(TokenStream source, params NonTerminal[] series) =>
        _MatchSeries(source, series).ToList();

    /// <summary>
    /// Checks a set of non-terminal actions, expecting at least one will
    /// successfully match.
    /// </summary>
    /// <param name="source">Stream from which tokens are consumed.</param>
    /// <param name="options">Possible next non-terminals to try matching.</param>
    /// <returns>Resulting DTO from any successfully matched member.</returns>
    protected object? MatchOptions(TokenStream source, params NonTerminal[] options)
    {
        // TODO Currently doesn't support backtracking, for now ensure unique
        // first sets.
        foreach (var nonTerm in options)
        {
            if (nonTerm.TryMatch(source, out var dto))
            {
                return dto;
            }
        }
        Valid = false;
        return null;
    }
}

/// <summary>
/// Generic wrapper for the regular non-terminal, allowing more strict typing
/// in abstract syntax definition.
/// </summary>
/// <typeparam name="T">DTO type corresponding to parsed out details.</typeparam>
public abstract class NonTerminal<T> : NonTerminal where T : class?
{
    public override bool TryMatch(TokenStream source, out object? dto)
    {
        var result = TryMatch(source, out T? _dto);
        dto = _dto;
        return result;
    }

    /// <summary>
    /// Attempts to parse the provided source stream as this type of non-
    /// terminal.
    /// </summary>
    /// <typeparam name="T">DTO type corresponding to parsed out details.</typeparam>
    /// <param name="source">Stream from which tokens are consumed.</param>
    /// <param name="dto">Object in which text literal details can be placed.</param>
    /// <returns>Whether the syntax was matched in a valid fashion.</returns>
    public abstract bool TryMatch(TokenStream source, out T? dto);
}