using Microsoft.AspNetCore.Identity;

namespace AuthAPI.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public List<SomfyDevice> SomfyDevices { get; set; } = new List<SomfyDevice>();
        public List<TuyaDevice> TuyaDevices { get; set; } = new List<TuyaDevice>();

        public List<SomfyEntity> SomfyEntities { get; set; } = new List<SomfyEntity>();

        public List<TuyaEntity> TuyaEntities { get; set; } = new List<TuyaEntity>();
    }
}
