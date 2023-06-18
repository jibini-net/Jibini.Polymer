using Jibini.Polymer.Prototype.Lexer;
using Jibini.Polymer.Prototype.Parser;

namespace Jibini.Polymer.Prototype.Grammar;

using static Token;

public class FunctionDto : StatementDto
{
    override public string Type => "Function";

    public string? Name { get; set; }
    public List<TypeDto>? TypeParams { get; set; }
    public List<ParameterDto>? Parameters { get; set; }
    public TypeDto? ReturnType { get; set; }
    public BodyDto? Body { get; set; }
}

public class Function : NonTerminal<FunctionDto>
{
    override public bool TryMatch(TokenStream source, out FunctionDto? dto)
    {
        var data = MatchSeries(source,
            Fun, new Ident()
            );
        dto = new()
        {
            Name = (data[1] as IdentDto)?.Name
        };

        dto.TypeParams = MatchOptions(source,
            new TypeParameters(),
            Epsilon)
            as List<TypeDto>;

        data = MatchSeries(source, new Parameters());
        dto.Parameters = data[0] as List<ParameterDto>;
        
        if (source.Next == Colon)
        {
            data = MatchSeries(source,
                Colon, new Type()
                );
            dto.ReturnType = data[1] as TypeDto;
        }

        data = MatchSeries(source, new Body());
        dto.Body = data[0] as BodyDto;
        return Valid;
    }
}