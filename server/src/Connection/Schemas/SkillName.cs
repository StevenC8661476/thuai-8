using System.Text.Json.Serialization;

namespace Thuai.Server.Connection.Schemas;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SkillName
{
        BLACK_OUT,
        SPEED_UP,
        FLASH,
        DESTROY,
        CONSTRUCT,
        TRAP,
        MISSILE,
        KAMUI
}
