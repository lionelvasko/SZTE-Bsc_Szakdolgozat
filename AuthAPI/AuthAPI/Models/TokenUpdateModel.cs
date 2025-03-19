namespace AuthAPI.Models
{
    public class TokenUpdateModel
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}
