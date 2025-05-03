using AuthAPI.Data;
using AuthAPI.DTOs;
using AuthAPI.Models;
using Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AuthAPI.Controllers
{
    /// <summary>
    /// Controller for managing main devices.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class MainDeviceController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly UserManager<User> userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainDeviceController"/> class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        /// <param name="userManager">The user manager for managing user-related operations.</param>
        public MainDeviceController(AppDbContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        /// <summary>
        /// Retrieves the list of devices associated with the current user.
        /// </summary>
        /// <returns>A list of <see cref="DeviceDTO"/> objects representing the user's devices.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeviceDTO>>> GetDevice()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var devices = await context.Devices
                .Where(d => d.UserId == userId)
                .ToListAsync();
            if (devices == null || !devices.Any()) return NotFound();
            var deviceDTOs = devices.Select(d => new DeviceDTO
            {
                Id = d.Id,
                Name = d.Name,
                CreationTime = d.CreationTime,
                Platform = d.Platform,
                TuyaEntities = [.. context.TuyaEntities.Where(e => e.DeviceId == d.Id).Select(te => new TuyaEntityDTO
                    {
                        Name = te.Name,
                        URL = te.Url,
                        Platform = te.Platform,
                        Icon = te.Icon,
                        AccessToken = te.AccessToken,
                        RefreshToken = te.RefreshToken,
                        Region = te.Region
                    })],
                SomfyEntities = [.. context.SomfyEntities.Where(e => e.DeviceId == d.Id).Select(se => new SomfyEntityDTO
                    {
                        Name = se.Name,
                        URL = se.Url,
                        Platform = se.Platform,
                        Icon = se.Icon,
                        BaseUrl = se.BaseUrl,
                        CloudUsername = se.CloudUsername,
                        CloudPasswordHashed = se.CloudPasswordHashed
                    })]
            }).ToList();
            return Ok(deviceDTOs);
        }

        /// <summary>
        /// Adds a new device for the current user.
        /// </summary>
        /// <param name="model">The device details to be added.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPost]
        public async Task<IActionResult> AddDevice(AddDeviceRequest model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return Unauthorized();
            var device = new Device
            {
                Name = model.Name,
                Platform = model.Platform,
                CreationTime = DateTime.UtcNow.ToString(),
                UserId = user.Id,
            };
            context.Devices.Add(device);
            await context.SaveChangesAsync();
            Guid id = device.Id;
            return CreatedAtAction(nameof(AddDevice), new { id = id }, new { id = id });
        }

        /// <summary>
        /// Deletes a device associated with the current user.
        /// </summary>
        /// <param name="id">The ID of the device to delete.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevice(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var device = await context.Devices.FirstOrDefaultAsync(d => d.Id == Guid.Parse(id) && d.UserId == userId);
            if (device == null) return NotFound();

            context.Devices.Remove(device);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}