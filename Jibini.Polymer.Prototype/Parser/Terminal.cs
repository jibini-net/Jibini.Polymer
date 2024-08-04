using Jibini.Polymer.Prototype.Lexer;

namespace Jibini.Polymer.Prototype.Parser;

/// <summary>
/// Allows a range of characters to be mapped to a leaf node within an abstract
/// syntax tree. Creates an object containing names, types, and other details.
/// </summary>
/// <typeparam name="T">DTO class which will be instantiated with details.</typeparam>
public class Terminal<T>(Token terminal) : NonTerminal<T> where T : class
{
    override public bool TryMatch(TokenStream source, out T dto)
    {
        if (source.Next != terminal)
        {
            dto = default;
            return Valid = false;
        }
        // Don't bother creating stub object instances
        if (typeof(T) == typeof(object))
        {
            dto = default;
            goto accept_and_exit;
        }

        // Output parameter will be populated if there is a constructor on the
        // DTO type accepting a single parameter which is a stream of tokens.
        var factory = typeof(T).GetConstructor([typeof(TokenStream)]);
        dto = factory?.Invoke([source]) as T;
        // Can fall back to zero-argument constructor where state not needed
        if (dto is null)
        {
            factory = typeof(T).GetConstructor([]);
            dto = factory?.Invoke([]) as T;
        }

    accept_and_exit:
        source.Poll();
        return true;
    }
}