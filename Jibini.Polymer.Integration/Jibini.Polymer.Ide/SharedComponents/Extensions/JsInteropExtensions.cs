using Microsoft.JSInterop;

namespace Jibini.Polymer.Ide.SharedComponents;

public static class JsInteropExtensions
{
    public static async Task<IJSObjectReference> ImportAsync(this IJSRuntime js, string filePath) =>
        await js.InvokeAsync<IJSObjectReference>("import", $"./_content/Jibini.Polymer.Ide.SharedComponents/{filePath}?{DateTime.Now.Ticks}");
}
