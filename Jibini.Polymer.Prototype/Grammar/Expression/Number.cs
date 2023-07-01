using Jibini.Polymer.Prototype.Lexer;
using Jibini.Polymer.Prototype.Parser;

namespace Jibini.Polymer.Prototype.Grammar;

public class NumberDto : ExpressionDto
{
    public override string _Type => "Number";

    public decimal Value { get; set; }
    public string Suffix { get; set; } = "";

    public NumberDto()
    {
    }

    public NumberDto(TokenStream source)
    {
        var numberText = source.Text.TrimEnd('f', 'd', 'L');
        Value = decimal.Parse(numberText);
        Suffix = source.Text.Substring(numberText.Length);
    }
}

public class Number : Terminal<NumberDto>
{
    public Number() : base(Token.Number)
    {
    }

    public override bool TryMatch(TokenStream source, out NumberDto? dto)
    {
        try
        {
            if (!base.TryMatch(source, out dto))
            {
                goto reject_and_exit;
            }
        } catch (OverflowException)
        {
            goto reject_and_exit;
        }
        return Valid;

    reject_and_exit:
        dto = new();
        return Valid = false;
    }
}