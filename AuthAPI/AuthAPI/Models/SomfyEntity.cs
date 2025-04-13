namespace AuthAPI.Models
{
    public class SomfyEntity : Entity
    {
        public string BaseUrl { get; set; }
        public string CloudUsername { get; set; }
        public string CloudPasswordHashed { get; set; }
    }
}