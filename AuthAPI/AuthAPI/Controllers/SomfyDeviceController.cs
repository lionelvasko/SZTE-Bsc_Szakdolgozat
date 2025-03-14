using AuthAPI.Data;
using AuthAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuthAPI.Controllers
{
    [Route("api/somfy")]
    public class SomfyDeviceController : DeviceController<SomfyDevice>
    {
        public SomfyDeviceController(AppDbContext context) : base(context) { }
    }
}
