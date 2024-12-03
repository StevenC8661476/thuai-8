using System.Text.Json.Serialization;

namespace Thuai.Server.Connection.Schemas;

public record Fence
{
    [JsonPropertyName("position")]
    public required PositionInt Position { get; init; }

    [JsonPropertyName("health")]
    public required int Health { get; init; }
}
