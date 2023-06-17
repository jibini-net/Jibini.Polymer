using Jibini.Polymer.Prototype.Grammar;
using Jibini.Polymer.Prototype.Lexer;

namespace Jibini.Polymer.Prototype;

internal class Program
{
    static void Main(string[] args)
    {
        var sourceText = @"
fun HelloWorld()
{
}

fun FooBar(thing: A<Thing<float>>, other_thing: B<string>)
{
    fun internalFunc(pars: C) { }
}
            ".Trim();
        var source = new TokenStream(sourceText);
        var success = new Statements().TryMatch(source, out var dto);

        Console.WriteLine(success);
        Console.WriteLine(dto);
    }
}