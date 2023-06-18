using Jibini.Polymer.Prototype.Lexer;
using Jibini.Polymer.Prototype.Parser;

namespace Jibini.Polymer.Prototype.Grammar;

using static Token;

public class DeclarationDto : StatementDto
{
    override public string _Type => "Declaration";

    public string Name { get; set; } = "";
    public TypeDto? Type { get; set; }
    // TODO
    public IdentDto? InitialValue { get; set; }
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
            Name = (data[1] as IdentDto)?.Name ?? ""
        };

        if (source.Next == Colon)
        {
            data = MatchSeries(source,
                Colon, new Type()
                );
            dto.Type = (data[1] as TypeDto)!;
        }

        if (source.Next == Equal)
        {
            // TODO
            data = MatchSeries(source,
                Equal, new Ident()
                );
            dto.InitialValue = data[1] as IdentDto;
        }

        _ = MatchSeries(source, Semic);

        Valid &= (dto.Type is not null || dto.InitialValue is not null);
        return Valid;
    }
}