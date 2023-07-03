using Jibini.Polymer.Prototype.Lexer;
using Jibini.Polymer.Prototype.Parser;

namespace Jibini.Polymer.Prototype.Grammar;

using static Token;

public class ForEachDto : StatementDto
{
    public override string _Type => "ForEach";

    public DeclarationDto Declaration { get; set; } = new();
    public ExpressionDto? Items { get; set; }
    public BodyDto Body { get; set; } = new();
}

public class ForEach : NonTerminal<ForEachDto>
{
    public override bool TryMatch(TokenStream source, out ForEachDto? dto)
    {
        dto = new();
        if (source.Next != Token.For)
        {
            return Valid = false;
        }
        var data = MatchSeries(source,
            Token.For, LParens, Var, new Ident()
            );
        dto.Declaration.Name = (data[3] as IdentDto)!;

        if (source.Next == Colon)
        {
            data = MatchSeries(source,
                Colon, new Type()
                );
            dto.Declaration.Type = data[1] as TypeDto;
        }

        data = MatchSeries(source,
            In, new Expression(), RParens,
            new Body()
            );
        dto.Items = data[1] as ExpressionDto;
        dto.Body = (data[3] as BodyDto)!;
        return Valid;
    }
}