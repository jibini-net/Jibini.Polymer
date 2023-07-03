using Jibini.Polymer.Prototype.Lexer;
using Jibini.Polymer.Prototype.Parser;

namespace Jibini.Polymer.Prototype.Grammar;

using static Token;

public class IdentDto : ExpressionDto
{
    public override string _Type => "Ident";

    public string Name { get; set; } = "";
    public List<TypeDto>? TypeParams { get; set; }

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

    public override bool TryMatch(TokenStream source, out IdentDto? dto)
    {
        if (!base.TryMatch(source, out dto))
        {
            dto = new();
            return Valid = false;
        }

        if (source.Next == Lt)
        {
            var data = MatchSeries(source, new TypeParams());
            dto!.TypeParams = data[0] as List<TypeDto>;
        }
        return Valid;
    }
}