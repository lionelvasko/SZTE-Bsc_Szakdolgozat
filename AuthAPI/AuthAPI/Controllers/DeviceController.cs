using AuthAPI.Data;
using AuthAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Swashbuckle.AspNetCore.Annotations;
using System.Diagnostics;

namespace AuthAPI.Controllers
{
    [ApiController]
    [Route("deviceapi/")]
    public class DeviceController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public DeviceController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        [HttpPost("addDevice")]
        [Authorize]
        [SwaggerOperation(Summary = "Add a new device", Description = "Adds a new device for the authenticated user.")]
        [SwaggerResponse(200, "Device added successfully", typeof(Device))]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(401, "Unauthorized")]
        public async Task<IActionResult> AddDevice([FromBody] Device device)
        {
            if (device == null)
            {
                return BadRequest("Device data is required.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized("User not found.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            device.UserId = userId;

            if (device.Platform == "tuya")
            {
                var tuyaDevice = new TuyaDevice
                {
                    Id = device.Id,
                    CreationTime = device.CreationTime,
                    Platform = device.Platform,
                    UserId = device.UserId,
                    Entities = device.Entities
                };

                _context.TuyaDevices.Add(tuyaDevice);
            }
            else if (device.Platform == "somfy")
            {
                var somfyDevice = device as SomfyDevice;
                if (somfyDevice == null)
                {
                    return BadRequest("Invalid Somfy device data.");
                }
                _context.SomfyDevices.Add(somfyDevice);
            }
            else
            {
                return BadRequest("Unknown device type.");
            }

            user.Devices.Add(device);
            await _context.SaveChangesAsync();
            return Ok(device.Id);
        }
    }
}