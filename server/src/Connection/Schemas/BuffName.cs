using System.Text.Json.Serialization;

namespace Thuai.Server.Connection.Schemas;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum BuffName
{
        BULLET_COUNT,
        BULLET_SPEED,
        ATTACK_SPEED,
        LASER,
        DAMAGE,
        ANTI_ARMOR,
        ARMOR,
        REFLECT,
        DODGE,
        KNIFE,
        GRAVITY,
        BLACK_OUT,
        SPEED_UP,
        FLASH,
        DESTROY,
        CONSTRUCT,
        TRAP,
        MISSILE,
        KAMUI
}
