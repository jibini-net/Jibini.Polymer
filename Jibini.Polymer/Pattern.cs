using System.Text.RegularExpressions;

namespace Jibini.Polymer;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class PatternAttribute : Attribute
{
    public string Regex { get; set; } = ".*";
}

public static class PatternExtensions
{
    private static Dictionary<string, Regex> cachedPatterns = new();

    static PatternExtensions()
    {
        // Ensure all patterns are compiled at startup to avoid issue of
        // concurrent dictionary reads/writes.
        _ = Enum.GetValues<Token>().SelectMany(GetPatterns).ToList();
    }

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

    public static IList<Regex> GetPatterns(this Token token)
    {
        var attributes = typeof(Token).GetField(token.ToString())!
            .GetCustomAttributes(typeof(PatternAttribute), false)
            as PatternAttribute[];
        return (from it in attributes select it.Regex.CompileMemoized())
            .ToList();
    }
}