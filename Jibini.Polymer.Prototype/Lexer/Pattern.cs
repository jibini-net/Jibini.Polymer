using System.Text.RegularExpressions;

namespace Jibini.Polymer.Prototype.Lexer;

/// <summary>
/// Marks that the annotated token enum is associated with a regex pattern.
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class PatternAttribute : Attribute
{
    /// <summary>
    /// Regular expression pattern which matches the annotated token.
    /// </summary>
    public string Regex { get; set; } = ".*";
}

public static class PatternExtensions
{
    /// <summary>
    /// Cache separate from the size-limited .NET cache. Should allow compiled
    /// patterns to be reused through runtime.
    /// </summary>
    private static Dictionary<string, Regex> cachedPatterns = new();

    static PatternExtensions()
    {
        // Ensure all patterns are compiled at startup to avoid issue of
        // concurrent dictionary reads/writes.
        _ = Enum.GetValues<Token>().SelectMany(GetPatterns).ToList();
    }

    /// <summary>
    /// Retrieves an existing compiled pattern, or compiles the provided regex
    /// into a pattern.
    /// </summary>
    /// <param name="pattern">Regular expression of the source token.</param>
    /// <returns>Compiled pattern which, possibly from the cache.</returns>
    public static Regex CompileMemoized(this string pattern)
    {
        if (cachedPatterns.TryGetValue(pattern, out var regex))
        {
            return regex;
        }
        // Should only ever reach here on initial static class load
        cachedPatterns[pattern] = new(pattern, RegexOptions.Compiled);
        return cachedPatterns[pattern];
    }

    /// <summary>
    /// Uses reflection to find any regex patterns associated with a token.
    /// </summary>
    /// <param name="token">Token enum to check for regex patterns.</param>
    /// <returns>Set of compiled regex patterns in order of precedence.</returns>
    public static IList<Regex> GetPatterns(this Token token)
    {
        var attributes = typeof(Token).GetField(token.ToString())!
            .GetCustomAttributes(typeof(PatternAttribute), false)
            as PatternAttribute[];
        return (from it in attributes select it.Regex.CompileMemoized())
            .ToList();
    }
}