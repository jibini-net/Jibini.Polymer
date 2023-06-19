using Jibini.Polymer.Prototype.Grammar;
using Jibini.Polymer.Prototype.Lexer;

namespace Jibini.Polymer.Prototype;

internal class Program
{
    static void Main(string[] _)
    {
        // Ensure tokens are compiled beforehand
        ".*".CompileMemoized();

        var time = DateTime.Now;
        var success = new Statements(endToken: null)
            .TryMatch(@"

var container: Container<dynamic> = new;
var zero: int;
var globalCount = zero;

fun DoWork(b: int)
{
    var i: int;
    fun changeValue(): int
    {
        i = globalCount + b;
        return = i;
    }
    container.Value = changeValue;
    container.Value();
}

                ",
                out var _);

        Console.WriteLine($"Completed in {(DateTime.Now - time).TotalMilliseconds}ms");
        Console.WriteLine($"Valid: {success}");
    }
}