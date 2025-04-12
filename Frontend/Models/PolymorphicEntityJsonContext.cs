using System.Text.Json.Serialization;

namespace Szakdoga.Models
{
    [JsonSourceGenerationOptions(WriteIndented = true)]
    [JsonSerializable(typeof(List<Entity>))]
    public partial class PolymorphicEntityJsonContext : JsonSerializerContext
    {
    }
}
