using Jibini.Polymer.Prototype.Grammar;
using Jibini.Polymer.Prototype.Lexer;

namespace Jibini.Polymer;

internal class Program
{
    static void Main(string[] args)
    {
        var sourceText = "fun \n/*Hello, world!*/Main(         \n) {}";
        var source = new TokenStream(sourceText);
        var success = new Function().TryMatch(source, out var dto);

        Console.WriteLine($"Input Source: '{sourceText.Replace("\n", "\\n")}'");
        Console.WriteLine($"Valid Parse: {success}");
        Console.WriteLine($"Function Name: {dto?.Ident?.Name}");
    }
}