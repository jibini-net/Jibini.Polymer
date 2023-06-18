using Jibini.Polymer.Prototype.Grammar;
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
    /// <summary>
    /// Allows tokens to be placed inline with non-terminals in a collection.
    /// </summary>
    /// <param name="token">Token enum value to match in source.</param>
    public static implicit operator NonTerminal(Token token) =>
        new Terminal<object?>(token);

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
            if (!Valid)
            {
                yield return null;
                continue;
            }
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

    protected object? MatchOptionsIgnoreInvalid(TokenStream source, params NonTerminal[] options)
    {
        var restorePoint = source.Offset;
        foreach (var nonTerm in options)
        {
            if (nonTerm.TryMatch(source, out var dto))
            {
                return dto;
            } else
            {
                // "Backtracking"
                source.Offset = restorePoint;
            }
        }
        Valid = false;
        return null;
    }

    /// <summary>
    /// Checks a set of non-terminal actions, expecting at least one will
    /// successfully match.
    /// </summary>
    /// <param name="source">Stream from which tokens are consumed.</param>
    /// <param name="options">Possible next non-terminals to try matching.</param>
    /// <returns>Resulting DTO from any successfully matched member.</returns>
    protected object? MatchOptions(TokenStream source, params NonTerminal[] options)
    {
        // After first failure, attempt no further matches
        if (!Valid)
        {
            return null;
        }
        return MatchOptionsIgnoreInvalid(source, options);
    }

    /// <summary>
    /// Represents an option which will match blankness (always matches), and
    /// always has the null-DTO.
    /// </summary>
    protected NonTerminal Epsilon => new Epsilon();
}

/// <summary>
/// Generic wrapper for the regular non-terminal, allowing more strict typing
/// in abstract syntax definition.
/// </summary>
/// <typeparam name="T">DTO type corresponding to parsed out details.</typeparam>
public abstract class NonTerminal<T> : NonTerminal where T : class?
{
    override public bool TryMatch(TokenStream source, out object? dto)
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