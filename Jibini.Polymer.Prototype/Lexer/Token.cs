namespace Jibini.Polymer.Prototype.Lexer;

using static SharedPatterns;

public static class SharedPatterns
{
    public const string LETTERS = "a|b|c|d|e|f|g|h|i|j|k|l|m|n|o|p|q|r|s|t|u|v|w|x|y|z";
    public const string LETTERS_UPPER = "A|B|C|D|E|F|G|H|I|J|K|L|M|N|O|P|Q|R|S|T|U|V|W|X|Y|Z";
    public const string NUMBERS = "0|1|2|3|4|5|6|7|8|9";
    public const string HEX = $"{NUMBERS}|a|b|c|d|e|f|A|B|C|D|E|F";
}

/// <summary>
/// Closed set of valid tokens in the input source.
/// </summary>
public enum Token
{
    // Whitespace is ignored
    [Pattern(Regex = "( \t\n\r)+")]
    // Block comments are ignored
    //TODO
    //[Pattern(Regex = "\\/\\*(\\*(?!\\/)|[^*])*\\*\\/")]
    Discard,

    [Pattern(Regex = "fun")]
    Fun,
    [Pattern(Regex = "var")]
    Var,
    [Pattern(Regex = "if")]
    If,
    [Pattern(Regex = "else")]
    Else,
    [Pattern(Regex = "while")]
    While,
    [Pattern(Regex = "for")]
    For,
    [Pattern(Regex = "in")]
    In,
    [Pattern(Regex = $"({LETTERS}|{LETTERS_UPPER}|_)(({LETTERS}|{LETTERS_UPPER}|{NUMBERS}|_)+|)")]
    Ident,

    //TODO
    //[Pattern(Regex = "0(b[01]{1,64}|x[0-9a-fA-F]{1,16})")]
    [Pattern(Regex = $"0(b(0|1)+|x({HEX})+)")]
    BinNumber,
    //[Pattern(Regex = "[0-9]+(.[0-9]+)?[fdL]?")]
    [Pattern(Regex = $"({NUMBERS})+(.({NUMBERS})+|)(f|d|L|)")]
    Number,
    //[Pattern(Regex = "\"([^\\\\\\\"]|\\\\.)*\"")]
    StringLit,

    [Pattern(Regex = "=>")]
    Arrow,

    [Pattern(Regex = ":")]
    Colon,
    [Pattern(Regex = "\\(")]
    LParens,
    [Pattern(Regex = "\\)")]
    RParens,
    [Pattern(Regex = ".")]
    Dot,
    [Pattern(Regex = "{")]
    LCurly,
    [Pattern(Regex = "}")]
    RCurly,
    [Pattern(Regex = "<")]
    Lt,
    [Pattern(Regex = ">")]
    Gt,
    [Pattern(Regex = ",")]
    Comma,
    [Pattern(Regex = ";")]
    Semic,
    [Pattern(Regex = "=")]
    Equal,
    [Pattern(Regex = "^")]
    Caret,
    [Pattern(Regex = "*")]
    Mult,
    [Pattern(Regex = "/")]
    Div,
    [Pattern(Regex = "\\+")]
    Add,
    [Pattern(Regex = "-")]
    Sub,
    [Pattern(Regex = "%")]
    Mod,
    [Pattern(Regex = "!")]
    Not,
    [Pattern(Regex = "?")]
    Quest,

    [Pattern(Regex = "")]
    Unknown
}