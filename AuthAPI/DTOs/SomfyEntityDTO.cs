namespace AuthAPI.DTOs
{
    public class SomfyEntityDTO : EntityDTO
    {
        public string BaseUrl { get; set; }
        public string CloudUsername { get; set; }
        public string CloudPasswordHashed { get; set; }
    }
}
