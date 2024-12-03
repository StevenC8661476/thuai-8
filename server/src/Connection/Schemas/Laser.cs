using System.Text.Json.Serialization;

namespace Thuai.Server.Connection.Schemas;

public record Laser
{
    [JsonPropertyName("start")]
    public required Position Start { get; init; }

    [JsonPropertyName("end")]
    public required Position End { get; init; }
}
