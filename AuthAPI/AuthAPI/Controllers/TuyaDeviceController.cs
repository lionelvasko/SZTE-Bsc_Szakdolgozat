using AuthAPI.Data;
using AuthAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuthAPI.Controllers
{
    [Route("api/tuya")]
    public class TuyaDeviceController : DeviceController<TuyaDevice>
    {
        public TuyaDeviceController(AppDbContext context) : base(context) { }
    }
}
