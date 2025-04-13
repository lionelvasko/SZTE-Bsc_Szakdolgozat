using System.Text.Json.Serialization;

namespace TuyaAPI.Models
{
    internal class SimpleModeToken
    {
        [JsonPropertyName("result")]
        internal Result Result { get; set; } = new Result();

        [JsonPropertyName("success")]
        internal bool Success { get; set; }

        [JsonPropertyName("t")]
        internal long T { get; set; }

        [JsonPropertyName("tid")]
        internal string Tid { get; set; }

        public override string ToString()
        {
            return $"Result: {(Result != null ? Result.ToString() : "null")} Success: {Success} T: {T} Tid: {Tid}";
        }
    }
}
