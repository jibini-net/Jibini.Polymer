using Jibini.Polymer.Prototype.Grammar;
using Jibini.Polymer.Prototype.Lexer;
using Newtonsoft.Json;

namespace Jibini.Polymer.Prototype;

internal class Program
{
    static void Main(string[] args)
    {
        var sourceText = @"
fun HelloWorld<T>(a: int): IUserOf<T>
{
    var b = a;
    var c: int;
    var d: int = b;
}

fun FooBar(thing: A<Thing<float>>, other_thing: B<string, int>)
{
    fun internalFunc(pars: C) { }
}
            ".Trim();
        var source = new TokenStream(sourceText);
        var success = new Statements(endToken: null).TryMatch(source, out var dto);

        Console.WriteLine(success);
        Console.WriteLine(JsonConvert.SerializeObject(dto, Formatting.Indented));
    }
}