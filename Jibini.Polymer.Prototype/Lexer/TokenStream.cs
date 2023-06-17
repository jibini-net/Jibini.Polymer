﻿namespace Jibini.Polymer.Prototype.Lexer;

/// <summary>
/// Scans an in-memory text buffer for token matches, matching one token at a
/// time as enums.
/// </summary>
public class TokenStream
{
    // In-memory source and current position in characters
    private readonly string source;
    private int offset = 0;

    // Stores the last peeked token to only peek once
    private Token? token;

    private string? _Text;
    /// <summary>
    /// Matched string content of the next available token in the stream, taken
    /// directly from the string content matched by regex.
    /// </summary>
    public string Text { get { _ = Next; return _Text!; } }

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
    private Token _Peek()
    {
        // Tokens are ordered statically in the intended evaluation order
        foreach (var tok in Enum.GetValues<Token>())
        {
            var matched = tok.GetPatterns()
                .Select((it) => it.Match(source, offset))
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
            if (offset >= source.Length)
            {
                return null;
            }
            _Peek();
            if (token == Token.Discard)
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
        offset += Text.Length;
        token = null;
        return result ?? throw new Exception("Polled end of file");
    }
}