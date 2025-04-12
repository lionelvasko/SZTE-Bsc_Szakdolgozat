
using System.ComponentModel.DataAnnotations;

namespace Szakdoga.Requests
{
    public class AddDeviceRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Platform { get; set; }
    }
}
