using System.Text.Json.Serialization;

namespace Thuai.Server.Connection.Schemas;

public record Bullet
{
    [JsonPropertyName("position")]
    public required Position Position { get; init; }

    [JsonPropertyName("speed")]
    public required double Speed { get; init; }

    [JsonPropertyName("damage")]
    public required double Damage { get; init; }

    [JsonPropertyName("traveledDistance")]
    public required double TraveledDistance { get; init; }
}
