namespace AuthAPI.DTOs
{
    public class SomfyEntityDTO : EntityDTO
    {
        public string BaseUrl { get; set; }
        public string GatewayPin { get; set; }
        public string SessionId { get; set; }
        public string Token { get; set; }
    }
}
