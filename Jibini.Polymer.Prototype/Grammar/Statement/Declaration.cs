using Jibini.Polymer.Prototype.Lexer;
using Jibini.Polymer.Prototype.Parser;

namespace Jibini.Polymer.Prototype.Grammar;

using static Token;

public class DeclarationDto : StatementDto
{
    override public string _Type => "Declaration";

    public IdentDto Name { get; set; } = new();
    public TypeDto? Type { get; set; }
    public ExpressionDto? InitialValue { get; set; }
}

public class Declaration : NonTerminal<DeclarationDto>
{
    override public bool TryMatch(TokenStream source, out DeclarationDto? dto)
    {
        var data = MatchSeries(source,
            Var, new Ident()
            );
        dto = new()
        {
            Name = (data[1] as IdentDto)!
        };

        if (source.Next == Colon)
        {
            data = MatchSeries(source,
                Colon, new Type()
                );
            dto.Type = data[1] as TypeDto;
        }

        if (source.Next == Equal)
        {
            data = MatchSeries(source,
                Equal, new Expression()
                );
            dto.InitialValue = data[1] as ExpressionDto;
        }

        _ = MatchSeries(source, Semic);

        Valid &= (dto.Type is not null || dto.InitialValue is not null);
        return Valid;
    }
}