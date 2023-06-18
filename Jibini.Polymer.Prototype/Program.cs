using Jibini.Polymer.Prototype.Grammar;
using Jibini.Polymer.Prototype.Lexer;
using Newtonsoft.Json;

namespace Jibini.Polymer.Prototype;

internal class Program
{
    static void Main(string[] args)
    {
        var sourceText = @"
fun FooBar<A, B, C>(thing: A<B<C>>, other_thing: Map<string, int>)
{
    fun internalFunc(pars: C) { }
    var b = a;
    var c: int;
    var d: int = b;
}
            ".Trim();
        var source = new TokenStream(sourceText);
        var success = new Statements(endToken: null).TryMatch(source, out var dto);

        Console.WriteLine(success);
        Console.WriteLine(JsonConvert.SerializeObject(dto, Formatting.Indented));
    }
}