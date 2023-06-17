﻿namespace Jibini.Polymer;

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
    [Pattern(Regex = "fun")]
    Fun,
    [Pattern(Regex = "[A-Za-z_][A-Za-z0-9_]*")]
    Ident
}