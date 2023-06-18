using Jibini.Polymer.Prototype.Lexer;
using Jibini.Polymer.Prototype.Parser;

namespace Jibini.Polymer.Prototype.Grammar;

using static Token;

public class FunctionDto : StatementDto
{
    override public string Type => "Function";

    public string? Name { get; set; }
    public List<ParameterDto>? Parameters { get; set; }
    public Type? ReturnType { get; set; }
    public BodyDto? Body { get; set; }
}

public class Function : NonTerminal<FunctionDto>
{
    override public bool TryMatch(TokenStream source, out FunctionDto? dto)
    {
        var data = MatchSeries(source,
            Fun, new Ident(), new Parameters(),
            new Body()
            );
        dto = new()
        {
            Name = (data[1] as IdentDto)?.Name,
            Parameters = data[2] as List<ParameterDto>,
            Body = data[3] as BodyDto
        };
        return Valid;
    }
}