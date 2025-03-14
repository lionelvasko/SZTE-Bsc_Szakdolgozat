using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthAPI.Models
{
    public class SomfyDevice : Device
    {
        [Required]
        public string SomfyDeviceId { get; set; }  // Egyedi azonosító a Somfy API szerint

        [Required]
        public string GatewayId { get; set; }  // A Somfy gateway azonosítója
    }
}
