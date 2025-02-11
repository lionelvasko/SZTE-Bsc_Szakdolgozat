using System.Text.Json.Serialization;

namespace Szakdoga.Models
{
    internal class TahomaExecutionId
    {
        [JsonPropertyName("execId")]
        public required string Id { get; set; }
    }
}
