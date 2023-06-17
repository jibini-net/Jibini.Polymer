using Jibini.Polymer.Prototype.Lexer;
using Jibini.Polymer.Prototype.Parser;

namespace Jibini.Polymer;

using static Token;

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

public class Body : NonTerminal
{
    public override bool TryMatch(TokenStream source, out object? dto)
    {
        _ = MatchSeries(source,

            LCurly, RCurly

            );
        dto = default;
        return Valid;
    }
}

public class FunctionDto
{
    public IdentDto? Ident { get; set; }
}

public class Function : NonTerminal<FunctionDto>
{
    public override bool TryMatch(TokenStream source, out FunctionDto? dto)
    {
        var data = MatchSeries(source,

            Fun, To<IdentDto>(Ident), LParens, RParens,

            new Body()

            );
        dto = new()
        {
            Ident = data[1] as IdentDto
        };
        return Valid;
    }
}

internal class Program
{
    static void Main(string[] args)
    {
        var tokens = new List<Token>()
        {
            Fun, Ident, LParens, RParens, LCurly, RCurly
        };
        var function = new Function();
        var source = new TokenStream(tokens);
        var success = function.TryMatch(source, out var dto);
        
        Console.WriteLine(success);
        Console.WriteLine(dto);
    }
}