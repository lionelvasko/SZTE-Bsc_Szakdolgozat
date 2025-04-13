using System.Text.Json.Serialization;

namespace TuyaAPI.Models
{
    internal class Result
    {
        [JsonPropertyName("access_token")]
        internal string Access_token { get; set; }

        [JsonPropertyName("expire_time")]
        internal int Expire_time { get; set; }

        [JsonPropertyName("refresh_token")]
        internal string Refresh_token { get; set; }

        [JsonPropertyName("uid")]
        internal string Uid { get; set; }

        public override string ToString()
        {
            return $"Access token: {Access_token} Expire time: {Expire_time} Refresh token: {Refresh_token} Uid: {Uid}";
        }
    }

}
