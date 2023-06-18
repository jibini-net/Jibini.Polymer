namespace Jibini.Polymer.Prototype.Lexer;

/// <summary>
/// Closed set of valid tokens in the input source.
/// </summary>
public enum Token
{
    [Pattern(Regex = "\\{")]
    LCurly,
    [Pattern(Regex = "\\}")]
    RCurly,
    [Pattern(Regex = "\\(")]
    LParens,
    [Pattern(Regex = "\\)")]
    RParens,
    [Pattern(Regex = "\\<")]
    Lt,
    [Pattern(Regex = "\\>")]
    Gt,
    [Pattern(Regex = "\\,")]
    Comma,
    [Pattern(Regex = "\\:")]
    Colon,
    [Pattern(Regex = "\\;")]
    Semic,
    [Pattern(Regex = "\\=")]
    Equal,
    [Pattern(Regex = "fun")]
    Fun,
    [Pattern(Regex = "var")]
    Var,
    [Pattern(Regex = "[A-Za-z_][A-Za-z0-9_]*")]
    Ident,

    // Whitespace is ignored
    [Pattern(Regex = "\\s+")]
    // Block comments are ignored
    [Pattern(Regex = "\\/\\*(\\*(?!\\/)|[^*])*\\*\\/")]
    Discard
}