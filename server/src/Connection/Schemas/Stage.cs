using System.Text.Json.Serialization;

namespace Thuai.Server.Connection.Schemas;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Stage
{
    REST,
    BATTLE,
    END
}
