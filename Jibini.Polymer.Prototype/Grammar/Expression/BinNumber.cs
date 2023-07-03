using Jibini.Polymer.Prototype.Lexer;
using Jibini.Polymer.Prototype.Parser;

namespace Jibini.Polymer.Prototype.Grammar;

public class BinNumberDto : ExpressionDto
{
    public override string _Type => "BinNumber";

    public ulong Value { get; set; }
    public int BitsSpecified { get; set; }

    public BinNumberDto()
    {
    }

    public BinNumberDto(TokenStream source)
    {
        var bits = source.Text.Substring(2);
        switch (source.Text[1])
        {
            case 'x':
                Value = Convert.ToUInt64(bits, 16);
                BitsSpecified = bits.Length * 4;
                break;

            case 'b':
                Value = Convert.ToUInt64(bits, 2);
                BitsSpecified = bits.Length;
                break;
        }
    }
}

public class BinNumber : Terminal<BinNumberDto>
{
    public BinNumber() : base(Token.BinNumber)
    {
    }

    public override bool TryMatch(TokenStream source, out BinNumberDto? dto)
    {
        if (!base.TryMatch(source, out dto))
        {
            dto = new();
            return Valid = false;
        }
        return Valid;
    }
}