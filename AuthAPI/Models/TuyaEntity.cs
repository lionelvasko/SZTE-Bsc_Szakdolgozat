namespace AuthAPI.Models
{
    public class TuyaEntity : Entity
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Region { get; set; }
    }
}
