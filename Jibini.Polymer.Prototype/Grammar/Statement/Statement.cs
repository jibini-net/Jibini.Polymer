using Jibini.Polymer.Prototype.Lexer;
using Jibini.Polymer.Prototype.Parser;

namespace Jibini.Polymer.Prototype.Grammar;

using static Token;

public abstract class StatementDto
{
    public abstract string _Type { get; }
}

public class Statement : NonTerminal<StatementDto>
{
    override public bool TryMatch(TokenStream source, out StatementDto? dto)
    {
        dto  = MatchOptions(source,
            new Function(),
            new Declaration(),
            new IfElse(),
            new While(),
            new For(),
            //new ForEach(),
            // new [...](), ...,
            new Body(),
            new ExprStatement(),
            Semic)
            as StatementDto;
        return Valid;
    }
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
            var data = MatchSeries(source, new Statement());
            var _dto = data[0] as StatementDto;

            // Error recovery boundary, nobody's consuming tokens or bailing out
            // so discard this token
            if (source.Offset - startPos == 0)
            {
                source.Poll();
            } else if (_dto is not null)
            {
                dto.Add(_dto);
            }
        }
        return Valid;
    }
}