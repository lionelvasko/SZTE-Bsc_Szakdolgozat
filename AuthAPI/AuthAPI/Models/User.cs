using SomfyAPI.Models;

namespace AuthAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; } = "User";
        public List<Device> Devices { get; set; } = new();
        public List<Entity> Entities { get; set; } = new();
        public string Salt { get; set; } = "";
    }
}
