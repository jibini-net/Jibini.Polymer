﻿using Jibini.Polymer.Prototype.Lexer;
using Jibini.Polymer.Prototype.Parser;

namespace Jibini.Polymer.Prototype.Grammar;

using static Token;

public class FunctionDto : StatementDto
{
    override public string _Type => "Function";

    public IdentDto Name { get; set; } = new();
    public List<TypeDto>? TypeParams { get; set; }
    public List<ParameterDto> Parameters { get; set; } = new();
    public TypeDto? ReturnType { get; set; }
    public BodyDto Body { get; set; } = new();
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
            Name = (data[1] as IdentDto)!
        };

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

        data = MatchSeries(source, new Body());
        dto.Body = (data[0] as BodyDto)!;
        return Valid;
    }
}