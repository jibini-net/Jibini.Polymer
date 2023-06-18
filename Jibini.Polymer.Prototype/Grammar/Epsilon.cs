using Jibini.Polymer.Prototype.Lexer;
using Jibini.Polymer.Prototype.Parser;

namespace Jibini.Polymer.Prototype.Grammar;

public class Epsilon : NonTerminal
{
    public override bool TryMatch(TokenStream _, out object? dto)
    {
        dto = null;
        return Valid;
    }
}