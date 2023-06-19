using Jibini.Polymer.Prototype.Lexer;
using Jibini.Polymer.Prototype.Parser;

namespace Jibini.Polymer.Prototype.Grammar;

using static Token;

public abstract class StatementDto
{
    public abstract string _Type { get; }
}

public class Statements : NonTerminal<List<StatementDto>>
{
    private readonly Token? endToken;

    public Statements(Token? endToken)
    {
        this.endToken = endToken;
    }

    override public bool TryMatch(TokenStream source, out List<StatementDto>? dto)
    {
        dto = new();
        // Cascaded value should help protect against polling EOF
        while (/*Valid && */(source.Next ?? endToken) != endToken)
        {
            int startPos = source.Offset;
            var data = MatchOptions(source,
                new Function(),
                new Declaration(),
                new Body(),
                new ExprStatement(),
                Semic)
                as StatementDto;

            // Error recovery boundary, nobody's consuming tokens or bailing out
            // so discard this token
            if (source.Offset - startPos == 0)
            {
                source.Poll();
            } else if (data is not null)
            {
                dto.Add(data);
            }
        }
        return Valid;
    }
}