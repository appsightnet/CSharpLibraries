using System.Text.Encodings.Web;
using System.Text.Json;

namespace AppSightNet.Text.Json;

public static class JsonSerializerOptionsProvider
{
    public static JsonSerializerOptions DefaultOptions { get; } =
        new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
}
