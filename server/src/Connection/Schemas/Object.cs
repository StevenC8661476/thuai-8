using System.Text.Json;
using System.Text.Json.Serialization;

namespace Thuai.Server.Connection.Schemas;

using Laser = List<LaserItem>;

using Wall = PositionInt;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AvaliableObjectName
{
    // The names are lowercase in the api,
    // therefore the names are lowercase here.
    player,
    wall,
    fence,
    bullet,
    laser,
    buffname
}

[JsonConverter(typeof(ThuaiObjectConverter))]
public record Object
{
    [JsonPropertyName("name")]
    public required AvaliableObjectName Name { get; init; }

    [JsonPropertyName("data")]
    public required object Data { get; init; }
}

public class ThuaiObjectConverter : JsonConverter<Object>
{
    public override Object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var jsonDocument = JsonDocument.ParseValue(ref reader);
        var root = jsonDocument.RootElement;
        var name = JsonSerializer.Deserialize<AvaliableObjectName>(root.GetProperty("name").GetRawText());

        object data = name switch
        {
            AvaliableObjectName.player => JsonSerializer.Deserialize<Player>(root.GetProperty("data").GetRawText()) ??
                throw new JsonException("Cannot deserialize data as Player"),

            AvaliableObjectName.wall => JsonSerializer.Deserialize<Wall>(root.GetProperty("data").GetRawText()) ??
                throw new JsonException("Cannot deserialize data as Wall"),

            AvaliableObjectName.fence => JsonSerializer.Deserialize<Fence>(root.GetProperty("data").GetRawText()) ??
                throw new JsonException("Cannot deserialize data as Fence"),

            AvaliableObjectName.bullet => JsonSerializer.Deserialize<Bullet>(root.GetProperty("data").GetRawText()) ??
                throw new JsonException("Cannot deserialize data as Bullet"),

            AvaliableObjectName.laser => JsonSerializer.Deserialize<Laser>(root.GetProperty("data").GetRawText()) ??
                throw new JsonException("Cannot deserialize data as Laser"),

            AvaliableObjectName.buffname => JsonSerializer.Deserialize<BuffName>(root.GetProperty("data").GetRawText()),
            _ => throw new JsonException($"Unknown data type \"{name}\".")
        };

        return new Object
        {
            Name = name,
            Data = data
        };
    }

    public override void Write(Utf8JsonWriter writer, Object value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("name", value.Name.ToString());

        writer.WritePropertyName("data");
        switch (value.Name)
        {
            case AvaliableObjectName.player:
                JsonSerializer.Serialize(writer, (Player)value.Data, options);
                break;
            case AvaliableObjectName.wall:
                JsonSerializer.Serialize(writer, (Wall)value.Data, options);
                break;
            case AvaliableObjectName.fence:
                JsonSerializer.Serialize(writer, (Fence)value.Data, options);
                break;
            case AvaliableObjectName.bullet:
                JsonSerializer.Serialize(writer, (Bullet)value.Data, options);
                break;
            case AvaliableObjectName.laser:
                JsonSerializer.Serialize(writer, (Laser)value.Data, options);
                break;
            case AvaliableObjectName.buffname:
                JsonSerializer.Serialize(writer, (BuffName)value.Data, options);
                break;
            default:
                throw new JsonException($"Unknown data type \"{value.Name}\".");
        }

        writer.WriteEndObject();
    }
}
