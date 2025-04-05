namespace TuyaAPI.DTOs
{
    public class EntityDTO
    {
        public int DeviceId { get; set; }
        public string URL { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Platform { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
    }
}
