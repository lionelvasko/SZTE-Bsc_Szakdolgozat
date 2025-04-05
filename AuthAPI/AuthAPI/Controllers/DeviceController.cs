  using AuthAPI.Data;
using AuthAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Swashbuckle.AspNetCore.Annotations;
using AuthAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class DevicesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public DevicesController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all devices for the authenticated user.")]
        [SwaggerResponse(200, "List of devices", typeof(IEnumerable<Device>))]
        [SwaggerResponse(401, "Unauthorized")]
        [SwaggerResponse(404, "No devices found")]
        [SwaggerResponse(500, "Internal server error")]
        [ProducesResponseType(typeof(IEnumerable<Device>), 200)]
        public async Task<ActionResult<IEnumerable<Device>>> GetDevices()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _context.Devices
                .Include(d => d.TuyaEntities)
                .Include(d => d.SomfyEntities)
                .Where(d => d.UserId == userId)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Device>> AddDevice(AddDevice model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return Unauthorized();

            var device = new Device
            {
                Name = model.Name,
                Platform = model.Platform,
                CreationTime = DateTime.UtcNow.ToString(),
                UserId = user.Id,
                SomfyEntities = [],
                TuyaEntities = []
            };

            _context.Devices.Add(device);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDevices), new { id = device.Id }, device);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevice(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var device = await _context.Devices.FirstOrDefaultAsync(d => d.Id == id && d.UserId == userId);
            if (device == null) return NotFound();

            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}