namespace Jibini.Polymer.Prototype.Lexer;

/// <summary>
/// Closed set of valid tokens in the input source.
/// </summary>
public enum Token
{
    // Whitespace is ignored
    [Pattern(Regex = "\\s+")]
    // Block comments are ignored
    [Pattern(Regex = "\\/\\*(\\*(?!\\/)|[^*])*\\*\\/")]
    Discard,

    [Pattern(Regex = "fun(?=\\s|$)")]
    Fun,
    [Pattern(Regex = "var(?=\\s|$)")]
    Var,
    [Pattern(Regex = "if(?=\\s|$)")]
    If,
    [Pattern(Regex = "else(?=\\s|$)")]
    Else,
    [Pattern(Regex = "while(?=\\s|$)")]
    While,
    [Pattern(Regex = "for(?=\\s|$)")]
    For,
    [Pattern(Regex = "in(?=\\s|$)")]
    In,
    [Pattern(Regex = "[A-Za-z_][A-Za-z0-9_]*")]
    Ident,

    [Pattern(Regex = "0(b[01]{1,64}|x[0-9a-fA-F]{1,16})")]
    BinNumber,
    [Pattern(Regex = "[0-9]+(.[0-9]+)?[fdL]?")]
    Number,
    [Pattern(Regex = "\"([^\\\\\\\"]|\\\\.)*\"")]
    StringLit,

    [Pattern(Regex = "\\=\\>")]
    Arrow,

    [Pattern(Regex = "\\:")]
    Colon,
    [Pattern(Regex = "\\(")]
    LParens,
    [Pattern(Regex = "\\)")]
    RParens,
    [Pattern(Regex = "\\.")]
    Dot,
    [Pattern(Regex = "\\{")]
    LCurly,
    [Pattern(Regex = "\\}")]
    RCurly,
    [Pattern(Regex = "\\<")]
    Lt,
    [Pattern(Regex = "\\>")]
    Gt,
    [Pattern(Regex = "\\,")]
    Comma,
    [Pattern(Regex = "\\;")]
    Semic,
    [Pattern(Regex = "\\=")]
    Equal,
    [Pattern(Regex = "\\^")]
    Caret,
    [Pattern(Regex = "\\*")]
    Mult,
    [Pattern(Regex = "\\/")]
    Div,
    [Pattern(Regex = "\\+")]
    Add,
    [Pattern(Regex = "\\-")]
    Sub,
    [Pattern(Regex = "\\%")]
    Mod,
    [Pattern(Regex = "\\!")]
    Not,
    [Pattern(Regex = "\\?")]
    Quest,

    [Pattern(Regex = ".")]
    Unknown
}