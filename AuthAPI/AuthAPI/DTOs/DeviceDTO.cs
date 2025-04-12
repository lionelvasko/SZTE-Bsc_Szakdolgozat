namespace AuthAPI.DTOs
{
    public class DeviceDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string CreationTime { get; set; } = string.Empty;
        public string Platform { get; set; } = string.Empty;
        public ICollection<SomfyEntityDTO> SomfyEntities { get; set; } = new List<SomfyEntityDTO>();
        public ICollection<TuyaEntityDTO> TuyaEntities { get; set; } = new List<TuyaEntityDTO>();
    }
}
