namespace Jibini.Polymer.Prototype.Lexer;

public class TokenStream
{
    private readonly List<Token> tokens;

    public TokenStream(List<Token> tokens)
    {
        this.tokens = tokens;
    }

    private Token _Peek()
    {
        _Text = "Hello, world!";
        return tokens.FirstOrDefault();
    }

    public Token? Next
    {
        get
        {
            if (token is not null)
            {
                return token!.Value;
            }
            return _Peek();
        }
    }

    public Token Poll()
    {
        var result = Next;
        token = null;
        tokens.RemoveAt(0);
        return result ?? throw new Exception("Polled end of file");
    }

    private Token? token;

    private string? _Text;
    public string Text { get { _ = Next; return _Text!; } }
}