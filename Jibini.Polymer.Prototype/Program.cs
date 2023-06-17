using Jibini.Polymer.Prototype.Grammar;
using Jibini.Polymer.Prototype.Lexer;

namespace Jibini.Polymer;

internal class Program
{
    static void Main(string[] args)
    {
        var sourceText = @"
fun HelloWorld()
{
}

fun FooBar()
{
}
            ".Trim();
        var source = new TokenStream(sourceText);

        for (var i = 0; i < 2; i++)
        {
            var success = new Function().TryMatch(source, out var dto);

            Console.WriteLine($"Input Source: '{sourceText.Replace("\n", "\\n").Replace("\r", "")}'");
            Console.WriteLine($"Valid Parse: {success}");
            Console.WriteLine($"Function Name: {dto?.Ident?.Name}");
            Console.WriteLine();
        }
    }
}