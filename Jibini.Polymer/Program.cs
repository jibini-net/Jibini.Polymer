namespace Jibini.Polymer;

public class IdentDto
{
    public string Name { get; set; }

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

         // {             }
            Token.LCurly, Token.RCurly
            
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
    private readonly Terminal<IdentDto> ident = new(Token.Ident);
    private readonly Body body = new();

    public override bool TryMatch(TokenStream source, out FunctionDto? dto)
    {
        var data = MatchSeries(source,

         // fun        Ident  (              )
            Token.Fun, ident, Token.LParens, Token.RParens,
         // [Body]
            body

            ).ToList();
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
            Token.Fun, Token.Ident, Token.LParens, Token.RParens,
            Token.LCurly, Token.RCurly,
        };
        var body = new Function();
        var source = new TokenStream(tokens);
        var success = body.TryMatch(source, out var dto);
        
        Console.WriteLine(success);
        Console.WriteLine(dto);
    }
}