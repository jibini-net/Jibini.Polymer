using System.Runtime.CompilerServices;

namespace Jibini.Polymer.Prototype.Lexer;

/// <summary>
/// Substring of input source mapped to a non-terminal and DTO. Can be used in
/// combination with the original source to find the matched substring.
/// </summary>
public class TokenMatch
{
    public Token Token { get; set; }
    public int Index { get; set; }
    public int Length { get; set; }
    public object? AstNode { get; set; }
    public object? AstDto { get; set; }
}

/// <summary>
/// Scans an in-memory text buffer for token matches, matching one token at a
/// time as enums.
/// </summary>
public class TokenStream
{
    /// <summary>
    /// Allows a string to be used automatically as a "token stream."
    /// </summary>
    /// <param name="sourceText">Text content of the input source.</param>
    public static implicit operator TokenStream(string sourceText) =>
        new(sourceText);

    // In-memory copy of source which will be parsed
    private readonly string source;
    /// <summary>
    /// Provides access to a substring of remaining source, primarily for
    /// debugging purposes.
    /// </summary>
    public string Remaining => source.Substring(Offset);

    private int offset = 0;
    /// <summary>
    /// Character position within the input source. Can be manually set.
    /// </summary>
    public int Offset
    {
        get => offset;
        set 
        {
            offset = value;
            token = null;
            _ = Next;
        }
    }

    // Stores the last peeked token to only peek once
    private Token? token;
    // Previous tokens, allowing source to be mapped to the AST
    public List<TokenMatch> SourceMap { get; set; } = new();

    private string? _Text;
    /// <summary>
    /// Matched string content of the next available token in the stream, taken
    /// directly from the string content matched by regex.
    /// </summary>
    public string Text { get { _ = Next; return _Text!; } }

    /// <summary>
    /// Whether or not to skip over discarded tokens, or to present them.
    /// </summary>
    public bool SkipDiscard { get; set; } = true;

    public TokenStream(string source)
    {
        this.source = source;
    }

    /// <summary>
    /// Attempts to match the highest precedence token possible for the input
    /// source text provided.
    /// </summary>
    /// <returns>A matched token at the current stream position.</returns>
    /// <exception cref="Exception">If no token could be matched here.</exception>
    private Token? _Peek()
    {
        if (Offset >= source.Length)
        {
            // EOF
            return null;
        }
        // Tokens are ordered statically in the intended evaluation order
        foreach (var tok in Enum.GetValues<Token>())
        {
            var matched = tok.GetPatterns()
                .Select((it) => it.Match(source, Offset))
                .FirstOrDefault((it) => it.Success);
            if (matched is not null)
            {
                _Text = matched.Value;
                token = tok;
                return token!.Value;
            }
        }
        throw new Exception("Unexpected content");
    }

    /// <summary>
    /// Peeks the stream of tokens to parse the next token without polling it,
    /// resulting in null if the stream has reached EOF.
    /// </summary>
    public Token? Next
    {
        get
        {
            // Resort to stored value to reduce duplicate work
            if (token is not null)
            {
                return token!.Value;
            }
            // Reached EOF condition
            if (Offset >= source.Length)
            {
                return null;
            }
            _Peek();
            if (SkipDiscard && token == Token.Discard)
            {
                // Discard whitespace and make recursive call
                Poll();
                return Next;
            }
            return token!.Value;
        }
    }

    /// <summary>
    /// Determines the next token at the current stream position, removing it
    /// and moving forward the stream's position by the length of the match.
    /// </summary>
    /// <returns>Matched token at the original stream position.</returns>
    /// <exception cref="Exception">If EOF is reached, but the stream is polled anyway.</exception>
    public Token Poll()
    {
        var result = Next;
        Offset += Text.Length;
        token = null;
        return result ?? throw new Exception("Polled end of file");
    }

    public async IAsyncEnumerable<(int, string, Token)> TokenizeAsync([EnumeratorCancellation] CancellationToken cancel)
    {
        for (; await Task.Run(() => Next) is not null; Poll())
        {
            if (cancel.IsCancellationRequested)
            {
                yield break;
            }
            yield return (Offset, Text, Next!.Value);
        }
    }
}