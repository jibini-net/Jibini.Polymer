using Jibini.Polymer.Prototype.Lexer;
using Jibini.Polymer.Prototype.Parser;

namespace Jibini.Polymer.Prototype.Grammar;

public class StringLitDto : ExpressionDto
{
    public override string _Type => "StringLit";

    public string Value { get; set; } = "";

    public StringLitDto()
    {
    }

    public StringLitDto(TokenStream source)
    {
        Value = source.Text.Substring(1, source.Text.Length - 2);
    }
}

public class StringLit : Terminal<StringLitDto>
{
    public StringLit() : base(Token.StringLit)
    {
    }

    public override bool TryMatch(TokenStream source, out StringLitDto? dto)
    {
        if (!base.TryMatch(source, out dto))
        {
            dto = new();
            return Valid = false;
        }
        return Valid;
    }
}