using Jibini.Polymer.Prototype.Lexer;
using Jibini.Polymer.Prototype.Parser;

namespace Jibini.Polymer.Prototype.Grammar;

using static Token;

public class FunctionDto : StatementDto
{
    override public string Type => "Function";

    public IdentDto? Ident { get; set; }
    public List<ParameterDto>? Parameters { get; set; }
    public BodyDto? Body { get; set; }
}

public class Function : NonTerminal<FunctionDto>
{
    public override bool TryMatch(TokenStream source, out FunctionDto? dto)
    {
        var data = MatchSeries(source,

            Fun, new Ident(), LParens, new Parameters(), RParens,

            new Body()

            );
        dto = new()
        {
            Ident = data[1] as IdentDto,
            Parameters = data[3] as List<ParameterDto>,
            Body = data[5] as BodyDto
        };
        return Valid;
    }
}