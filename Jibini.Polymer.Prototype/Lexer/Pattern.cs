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
