using AuthAPI.Data;
using AuthAPI.DTOs;
using AuthAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AuthAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EntityController(AppDbContext context, UserManager<User> userManager) : ControllerBase
    {

        // A device-hoz tartozó entitások lekérésére
        [HttpGet("Device/{deviceId}")]
        public async Task<ActionResult<IEnumerable<Entity>>> GetDeviceEntities(int deviceId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var device = await context.Devices.FirstOrDefaultAsync(d => d.Id == deviceId && d.UserId == userId);
            if(device == null) return NotFound();
            if (device.Platform == "Tuya")
            {
                var TuyaEntities = await context.TuyaEntities
                    .Where(e => e.DeviceId == deviceId && e.UserId == userId)
                    .ToListAsync();
                return Ok(TuyaEntities);
            }
            else if (device.Platform == "Somfy")
            {
                var SomfyEntities = await context.SomfyEntities
                    .Where(e => e.DeviceId == deviceId && e.UserId == userId)
                    .ToListAsync();
                return Ok(SomfyEntities);
            }
            return NotFound();
        }

        // A userhez tartozó device-ok entitásainak lekérésére
        [HttpGet("UserEntities")]
        public async Task<ActionResult<IEnumerable<Entity>>> GetUsersEntities()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var devices = await context.Devices
                .Where(d => d.UserId == userId)
                .ToListAsync();

            if (devices == null || !devices.Any()) return NotFound();
            var entities = new List<Entity>();
            foreach (var entity in devices)
            {
                var tuyaEntities = context.TuyaEntities
                    .Where(e => e.DeviceId == entity.Id && e.UserId == userId)
                    .ToList();
                entities.AddRange(tuyaEntities);

                var somfyEntities = context.SomfyEntities
                    .Where(e => e.DeviceId == entity.Id && e.UserId == userId)
                    .ToList();
                entities.AddRange(somfyEntities);
            }
            return Ok(entities);
        }

        // Új entitás hozzáadása a device-hoz
        [HttpPost]
        public async Task<ActionResult<Entity>> AddEntity(int deviceId, AddEntity model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userManager.FindByIdAsync(userId);
            var device = await context.Devices.FirstOrDefaultAsync(d => d.Id == deviceId && d.UserId == userId && d.Platform == model.Platform);

            if (device == null) return NotFound();

            if (model.Platform == "Tuya")
            {
                var entity = new TuyaEntity
                {
                    URL = model.URL,
                    Name = model.Name,
                    Platform = model.Platform,
                    Icon = model.Icon,
                    DeviceId = device.Id,
                    UserId = user.Id,
                    AccessToken = model.AccessToken,
                    RefreshToken = model.RefreshToken,
                    Region = model.Region
                };
                context.TuyaEntities.Add(entity);
                await context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetDeviceEntities), new { deviceId = device.Id }, entity);
            }
            else if (model.Platform == "Somfy") 
            {
                var entity = new SomfyEntity
                {
                    URL = model.URL,
                    Name = model.Name,
                    Platform = model.Platform,
                    Icon = model.Icon,
                    DeviceId = device.Id,
                    UserId = user.Id,
                    BaseUrl = model.BaseUrl,
                    GatewayPin = model.GatewayPin,
                    SessionId = model.SessionId,
                    Token = model.Token
                };
                context.SomfyEntities.Add(entity);
                await context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetDeviceEntities), new { deviceId = device.Id }, entity);
            }
            return BadRequest("Unsupported platform");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntity(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var tuyaEntity = await context.TuyaEntities.FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);
            if (tuyaEntity == null)
            {
                var somfyEntity = await context.SomfyEntities.FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);
                if (somfyEntity == null)
                {
                    return NotFound();
                }
                else
                {
                    context.SomfyEntities.Remove(somfyEntity);
                }  
            }
            else
            {
                context.TuyaEntities.Remove(tuyaEntity);
            }
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
