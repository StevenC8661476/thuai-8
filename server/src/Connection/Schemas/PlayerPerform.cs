using System.Text.Json;
using System.Text.Json.Serialization;

namespace Thuai.Server.Connection.Schemas;

#region Enums

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MoveDirection
{
    BACK,
    FORTH
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TurnDirection
{
    CLOCKWISE,
    COUNTER_CLOCKWISE
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PlayerType
{
    SELF,
    OPPONENT
}

#endregion

#region Converter

public class PlayerPerformConverter : JsonConverter<PlayerPerform>
{
    public override PlayerPerform Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var jsonDocument = JsonDocument.ParseValue(ref reader);
        var root = jsonDocument.RootElement;
        string messageType = root.GetProperty("messageType").GetString() ??
            throw new JsonException("Key \"messageType\" is not found in the JSON object.");

        return messageType switch
        {
            "PERFORM_MOVE" => JsonSerializer.Deserialize<PerformMove>(root.GetRawText()) ??
                throw new JsonException("Failed to deserialize PERFORM_MOVE message."),
        
            "PERFORM_TURN" => JsonSerializer.Deserialize<PerformTurn>(root.GetRawText()) ??
                throw new JsonException("Failed to deserialize PERFORM_TURN message."),

            "PERFORM_ATTACK" => JsonSerializer.Deserialize<PerformAttack>(root.GetRawText()) ??
                throw new JsonException("Failed to deserialize PERFORM_ATTACK message."),

            "PERFORM_SKILL" => JsonSerializer.Deserialize<PerformSkill>(root.GetRawText()) ??
                throw new JsonException("Failed to deserialize PERFORM_SKILL message."),

            "PERFORM_SELECT" => JsonSerializer.Deserialize<PerformSelect>(root.GetRawText()) ??
                throw new JsonException("Failed to deserialize PERFORM_SELECT message."),

            "GET_PLAYER_INFO" => JsonSerializer.Deserialize<GetPlayerInfo>(root.GetRawText()) ??
                throw new JsonException("Failed to deserialize GET_PLAYER_INFO message."),

            "GET_ENVIRONMENT_INFO" => JsonSerializer.Deserialize<GetEnvironmentInfo>(root.GetRawText()) ??
                throw new JsonException("Failed to deserialize GET_ENVIRONMENT_INFO message."),

            "GET_GAME_STATISTICS" => JsonSerializer.Deserialize<GetGameStatistics>(root.GetRawText()) ?? 
                throw new JsonException("Failed to deserialize GET_GAME_STATISTICS message."),

            "GET_AVAILABLE_BUFFS" => JsonSerializer.Deserialize<GetAvailableBuffs>(root.GetRawText()) ??
                throw new JsonException("Failed to deserialize GET_AVAILABLE_BUFFS message."),

            _ => throw new JsonException($"Unknown action type: {(messageType == "" ? "[Empty]" : messageType)}")
        };
    }

    public override void Write(Utf8JsonWriter writer, PlayerPerform value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}

#endregion

#region BaseClass

[JsonConverter(typeof(PlayerPerformConverter))]
public record PlayerPerform
{
    // This class is used to deserialize the message from the client
    [JsonPropertyName("messageType")]
    public virtual required string MessageType { get; init; }
}

#endregion

#region Actions

public record PerformMove : PlayerPerform
{
    [JsonPropertyName("messageType")]
    public override required string MessageType { get; init; } = "PERFORM_MOVE";

    [JsonPropertyName("token")]
    public required string Token { get; init; }

    [JsonPropertyName("direction")]
    public required MoveDirection Direction { get; init; }
}

public record PerformTurn : PlayerPerform
{
    [JsonPropertyName("messageType")]
    public override required string MessageType { get; init; } = "PERFORM_TURN";

    [JsonPropertyName("token")]
    public required string Token { get; init; }

    [JsonPropertyName("direction")]
    public required TurnDirection Direction { get; init; }
}

public record PerformAttack : PlayerPerform
{
    [JsonPropertyName("messageType")]
    public override required string MessageType { get; init; } = "PERFORM_ATTACK";

    [JsonPropertyName("token")]
    public required string Token { get; init; }
}

public record PerformSkill : PlayerPerform
{
    [JsonPropertyName("messageType")]
    public override required string MessageType { get; init; } = "PERFORM_SKILL";

    [JsonPropertyName("token")]
    public required string Token { get; init; }

    [JsonPropertyName("skillName")]
    public required SkillName SkillName { get; init; }
}

public record PerformSelect : PlayerPerform
{
    [JsonPropertyName("messageType")]
    public override required string MessageType { get; init; } = "PERFORM_SELECT";

    [JsonPropertyName("token")]
    public required string Token { get; init; }

    [JsonPropertyName("buffName")]
    public required BuffName BuffName { get; init; }
}

#endregion

#region Requests

public record GetPlayerInfo : PlayerPerform
{
    [JsonPropertyName("messageType")]
    public override required string MessageType { get; init; } =  "GET_PLAYER_INFO";

    [JsonPropertyName("token")]
    public required string Token { get; init; }

    [JsonPropertyName("request")]
    public required PlayerType Request { get; init; }
}

public record GetEnvironmentInfo : PlayerPerform
{
    [JsonPropertyName("messageType")]
    public override required string MessageType { get; init; } = "GET_ENVIRONMENT_INFO";

    [JsonPropertyName("token")]
    public required string Token { get; init; }
}

public record GetGameStatistics : PlayerPerform
{
    [JsonPropertyName("messageType")]
    public override required string MessageType { get; init; } = "GET_GAME_STATISTICS";

    [JsonPropertyName("token")]
    public required string Token { get; init; }
}

public record GetAvailableBuffs : PlayerPerform
{
    [JsonPropertyName("messageType")]
    public override required string MessageType { get; init; } = "GET_AVAILABLE_BUFFS";

    [JsonPropertyName("token")]
    public required string Token { get; init; }
}

#endregion
