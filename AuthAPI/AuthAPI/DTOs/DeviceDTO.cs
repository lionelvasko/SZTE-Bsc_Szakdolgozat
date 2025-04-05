namespace AuthAPI.DTOs
{
    public class DeviceDTO
    {
        public string Name { get; set; } = string.Empty;
        public string CreationTime { get; set; } = string.Empty;
        public string Platform { get; set; } = string.Empty;
        public ICollection<EntityDTO> Entities { get; set; } = new List<EntityDTO>();
    }
}
