using Jibini.Polymer.Prototype.Lexer;
using Jibini.Polymer.Prototype.Parser;

namespace Jibini.Polymer.Prototype.Grammar;

public class IdentDto
{
    public string Name { get; set; } = "";

    public IdentDto()
    {
    }

    public IdentDto(TokenStream source)
    {
        Name = source.Text;
    }
}

public class Ident : Terminal<IdentDto>
{
    public Ident() : base(Token.Ident)
    {
    }
}