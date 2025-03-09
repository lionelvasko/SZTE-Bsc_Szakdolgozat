using System.Text.Json.Serialization;

namespace TuyaAPI.Models
{
    internal class Result
    {
        [JsonPropertyName("access_token")]
        internal string Access_token { get; set; }
        [JsonPropertyName("expire_time")]
        internal string Expire_time { get; set; }
        [JsonPropertyName("refresh_token")]
        internal string Refresh_token { get; set; }
        [JsonPropertyName("uid")]
        internal string Uid { get; set; }
    }
}   
