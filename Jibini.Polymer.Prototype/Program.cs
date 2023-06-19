using Jibini.Polymer.Prototype.Grammar;
using Jibini.Polymer.Prototype.Lexer;
using Newtonsoft.Json;

namespace Jibini.Polymer.Prototype;

internal class Program
{
    static void Main(string[] args)
    {
        var sourceText = @"
fun FooBar<A, B, C>(thing: List<A, Map<B, C>>): string
{
    fun internalFunc(pars: C) { }

    var b = a;
    var c: int;
    var d: int = b;

    a; b; c; d;
    a.b = c(d).e(f, g);
    
    var test_a: string;
    test_a = a.ToString();
    var test_b = b.ToString();

    a.b = c^b.d^e()^f;
}

var test: int;
test = a * b.c()^!d.e + !thing / a^b^c + thing;
test = a * (b.c()^!(d.e + !thing / ((a^b)^c + thing)));
            ".Trim();
        var source = new TokenStream(sourceText);
        var success = new Statements(endToken: null).TryMatch(source, out var dto);

        Console.WriteLine(success);
        Console.WriteLine(JsonConvert.SerializeObject(dto, Formatting.Indented));
    }
}