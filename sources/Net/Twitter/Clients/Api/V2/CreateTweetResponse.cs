using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AppSightNet.Net.Twitter.Clients.Api.V2;

public class CreateTweetResponse
{
    [JsonPropertyName("data")]
    [Required]
    public Data Data { get; set; } = new Data();
}

public class Data
{
    [JsonPropertyName("id")]
    [Required]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("text")]
    [Required]
    public string Text { get; set; } = string.Empty;
}
