using System.Text.Json.Serialization;

namespace SomfyAPI.Models
{
    internal class TahomaExecutionId
    {
        [JsonPropertyName("execId")]
        public required string Id { get; set; }
    }
}
