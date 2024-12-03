using System.Text.Json.Serialization;

namespace Thuai.Server.Connection.Schemas;

public record Position
{
    [JsonPropertyName("x")]
    public required double X { get; init; }

    [JsonPropertyName("y")]
    public required double Y { get; init; }

    [JsonPropertyName("angle")]
    public required double Angle { get; init; }
}
