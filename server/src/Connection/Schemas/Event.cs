using System.Text.Json;
using System.Text.Json.Serialization;

namespace Thuai.Server.Connection.Schemas;

#region InnerSchemas

[JsonConverter(typeof(AppearEventPositionConverter))]
public record AppearEventPosition
{
    // I'd like to make it an enum,
    // but there is a lower-case 'object' in the api.
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("data")]
    public required object Data { get; init; }
}

#endregion

#region Schemas

[JsonConverter(typeof(EventConverter))]
public record Event
{
    [JsonPropertyName("messageType")]
    public virtual required string MessageType { get; init; }
}

public record AppearEvent : Event
{
    [JsonPropertyName("messageType")]
    public override required string MessageType { get; init; } = "APPEAR_EVENT";

    [JsonPropertyName("target")]
    public required Object Target { get; init; }

    // I don't know why this is called "position",
    // but as the api does, I will keep it.
    [JsonPropertyName("position")]
    public required AppearEventPosition Position { get; init; }
}

public record BobEvent : Event
{
    [JsonPropertyName("messageType")]
    public override required string MessageType { get; init; } = "BOB_EVENT";

    [JsonPropertyName("target")]
    public required Object Target { get; init; }

    [JsonPropertyName("end")]
    public required Position End { get; init; }
}

public record CollisionEvent : Event
{
    [JsonPropertyName("messageType")]
    public override required string MessageType { get; init; } = "COLLISION_EVENT";

    [JsonPropertyName("targets")]
    public required List<Object> Targets { get; init; }
}

public record DestroyEvent : Event
{
    [JsonPropertyName("messageType")]
    public override required string MessageType { get; init; } = "DESTROY_EVENT";

    [JsonPropertyName("target")]
    public required Object Target { get; init; }
}

#endregion

#region Converters

public class EventConverter : JsonConverter<Event>
{
    public override Event Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var jsonDocument = JsonDocument.ParseValue(ref reader);
        var root = jsonDocument.RootElement;
        var messageType = JsonSerializer.Deserialize<string>(root.GetProperty("messageType").GetRawText());
        return messageType switch
        {
            "APPEAR_EVENT" => JsonSerializer.Deserialize<AppearEvent>(root.GetRawText()) ??
                throw new JsonException("Cannot deserialize as AppearEvent"),

            "BOB_EVENT" => JsonSerializer.Deserialize<BobEvent>(root.GetRawText()) ??
                throw new JsonException("Cannot deserialize as BobEvent"),

            "COLLISION_EVENT" => JsonSerializer.Deserialize<CollisionEvent>(root.GetRawText()) ??
                throw new JsonException("Cannot deserialize as CollisionEvent"),

            "DESTROY_EVENT" => JsonSerializer.Deserialize<DestroyEvent>(root.GetRawText()) ??
                throw new JsonException("Cannot deserialize as DestroyEvent"),

            _ => throw new JsonException($"Unknown event type \"{(messageType == "" ? "[Empty]" : messageType)}\".")
        };
    }

    public override void Write(Utf8JsonWriter writer, Event value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("messageType", value.MessageType);

        switch (value.MessageType)
        {
            case "APPEAR_EVENT":
                JsonSerializer.Serialize(writer, (AppearEvent)value, options);
                break;
            case "BOB_EVENT":
                JsonSerializer.Serialize(writer, (BobEvent)value, options);
                break;
            case "COLLISION_EVENT":
                JsonSerializer.Serialize(writer, (CollisionEvent)value, options);
                break;
            case "DESTROY_EVENT":
                JsonSerializer.Serialize(writer, (DestroyEvent)value, options);
                break;
            default:
                throw new JsonException($"Unknown message type \"{value.MessageType}\".");
        }

        writer.WriteEndObject();
    }
}

public class AppearEventPositionConverter : JsonConverter<AppearEventPosition>
{
    public override AppearEventPosition Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var jsonDocument = JsonDocument.ParseValue(ref reader);
        var root = jsonDocument.RootElement;
        var name = JsonSerializer.Deserialize<string>(root.GetProperty("name").GetRawText());
        object data = name switch
        {
            "object" => JsonSerializer.Deserialize<Object>(root.GetProperty("data").GetRawText()) ??
                throw new JsonException("Cannot deserialize data as Object"),
            "position" => JsonSerializer.Deserialize<Position>(root.GetProperty("data").GetRawText()) ??
                throw new JsonException("Cannot deserialize data as PositionInt"),
            _ => throw new JsonException($"Unknown data type \"{name}\".")
        };
        return new AppearEventPosition
        {
            Name = name,
            Data = data
        };
    }

    public override void Write(Utf8JsonWriter writer, AppearEventPosition value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("name", value.Name);

        writer.WritePropertyName("data");
        switch (value.Name)
        {
            case "object":
                JsonSerializer.Serialize(writer, (Object)value.Data, options);
                break;
            case "position":
                JsonSerializer.Serialize(writer, (Position)value.Data, options);
                break;
            default:
                throw new JsonException($"Unknown data type \"{value.Name}\".");
        }

        writer.WriteEndObject();
    }
}

#endregion
