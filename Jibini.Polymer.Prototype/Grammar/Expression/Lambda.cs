using Jibini.Polymer.Prototype.Lexer;
using Jibini.Polymer.Prototype.Parser;

namespace Jibini.Polymer.Prototype.Grammar;

using static Token;

public class LambdaDto : ExpressionDto
{
    public override string _Type => "Lambda";

    public List<TypeDto>? TypeParams { get; set; }
    public List<ParameterDto> Parameters { get; set; } = new();
    public TypeDto? ReturnType { get; set; }
    public BodyDto Body { get; set; } = new();
}

public class Lambda : NonTerminal<LambdaDto>
{
    override public bool TryMatch(TokenStream source, out LambdaDto? dto)
    {
        IList<object?> data;
        dto = new();
        if (source.Next == Lt)
        {
            data = MatchSeries(source, new TypeParams());
            dto.TypeParams = data[0] as List<TypeDto>;
        }

        data = MatchSeries(source, new Parameters());
        dto.Parameters = (data[0] as List<ParameterDto>)!;

        if (source.Next == Colon)
        {
            data = MatchSeries(source,
                Colon, new Type()
                );
            dto.ReturnType = data[1] as TypeDto;
        }

        data = MatchSeries(source,
            Arrow, new Body()
            );
        dto.Body = (data[1] as BodyDto)!;
        return Valid;
    }
}