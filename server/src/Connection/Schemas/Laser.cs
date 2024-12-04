using System.Text.Json.Serialization;

namespace Thuai.Server.Connection.Schemas;

using Laser = List<LaserItem>;

public record LaserItem
{
    [JsonPropertyName("start")]
    public required Position Start { get; init; }

    [JsonPropertyName("end")]
    public required Position End { get; init; }
}
