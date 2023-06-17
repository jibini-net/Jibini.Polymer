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

fun FooBar(thing: A, other_thing: B)
{
}
            ".Trim();
        var source = new TokenStream(sourceText);

        for (var i = 0; i < 2; i++)
        {
            Console.WriteLine($"Input Source: '{sourceText.Substring(source.Offset)
                .Replace("\n", "\\n")
                .Replace("\r", "")}'");

            var success = new Function().TryMatch(source, out var dto);

            Console.WriteLine($"Valid Parse: {success}");
            Console.WriteLine($"Function Name: {dto?.Ident?.Name}");
            if ((dto?.Parameters?.Count ?? 0) == 0)
            {
                Console.WriteLine(" (No parameters)");
            }
            foreach (var param in dto?.Parameters ?? new())
            {
                Console.WriteLine($" - Param {param?.Ident?.Name} is of type {param?.Type?.Name}");
            }
        }
    }
}