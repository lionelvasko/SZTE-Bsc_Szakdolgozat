using System.Text.Json.Serialization;

namespace TuyaAPI.Models
{
    internal class SimpleModeToken
    {
        [JsonPropertyName("result")]
        internal Result Result { get; set; }
        [JsonPropertyName("success")]
        internal string Succes { get; set; }
        [JsonPropertyName("t")]
        internal string T { get; set; }
        [JsonPropertyName("tid")]
        internal string Tid { get; set; }
    }
}
