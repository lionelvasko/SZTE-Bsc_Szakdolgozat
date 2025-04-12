using AuthAPI.Data;
using AuthAPI.DTOs;
using AuthAPI.Models;
using AuthAPI.Requests;
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
        public async Task<ActionResult<IEnumerable<EntityDTO>>> GetDeviceEntities(string deviceId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var device = await context.Devices.FirstOrDefaultAsync(d => d.Id == Guid.Parse(deviceId) && d.UserId == userId);
            if (device == null) return NotFound();
            if (device.Platform == "Tuya")
            {
                var tuyaEntities = await context.TuyaEntities
                    .Where(e => e.DeviceId == Guid.Parse(deviceId) && e.UserId == userId)
                    .ToListAsync();
                if (tuyaEntities.Count == 0)
                {
                    return NotFound();
                }

                var DTOs = new List<TuyaEntityDTO>();
                foreach (var tuyaEntity in tuyaEntities)
                {
                    var entityDTO = new TuyaEntityDTO()
                    {
                        Name = tuyaEntity.Name,
                        URL = tuyaEntity.Url,
                        Platform = tuyaEntity.Platform,
                        Icon = tuyaEntity.Icon,
                        AccessToken = tuyaEntity.AccessToken,
                        RefreshToken = tuyaEntity.RefreshToken,
                        Region = tuyaEntity.Region
                    };
                    DTOs.Add(entityDTO);
                }
                return Ok(DTOs);
            }
            else if (device.Platform == "Somfy")
            {
                var somfyEntities = await context.SomfyEntities
                    .Where(e => e.DeviceId == Guid.Parse(deviceId) && e.UserId == userId)
                    .ToListAsync();
                var DTOs = new List<SomfyEntityDTO>();
                foreach (var somfyEntity in somfyEntities)
                {
                    var entityDTO = new SomfyEntityDTO()
                    {
                        Name = somfyEntity.Name,
                        URL = somfyEntity.Url,
                        Platform = somfyEntity.Platform,
                        Icon = somfyEntity.Icon,
                        BaseUrl = somfyEntity.BaseUrl,
                        Token = somfyEntity.Token,
                        GatewayPin = somfyEntity.GatewayPin,
                        SessionId = somfyEntity.SessionId
                    };
                    DTOs.Add(entityDTO);
                }
                return Ok(DTOs);
            }
            return NotFound();
        }

        // A userhez tartozó entitások lekérésére
        [HttpGet("UserEntities")]
        public async Task<ActionResult<IEnumerable<EntityDTO>>> GetUsersEntities()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var entities = new List<EntityDTO>();
            var seDTOs = new List<SomfyEntityDTO>();
            var teDTOs = new List<TuyaEntityDTO>();

            var somfyEntities = await context.SomfyEntities
                .Where(e => e.UserId == userId)
                .ToListAsync();
            foreach (var somfyEntity in somfyEntities)
            {
                var entityDTO = new SomfyEntityDTO()
                {
                    Name = somfyEntity.Name,
                    URL = somfyEntity.Url,
                    Platform = somfyEntity.Platform,
                    Icon = somfyEntity.Icon,
                    BaseUrl = somfyEntity.BaseUrl,
                    Token = somfyEntity.Token,
                    GatewayPin = somfyEntity.GatewayPin,
                    SessionId = somfyEntity.SessionId
                };
                seDTOs.Add(entityDTO);
            }
            var tuyaEntities = await context.TuyaEntities
                .Where(e => e.UserId == userId)
                .ToListAsync();
            foreach (var tuyaEntity in tuyaEntities)
            {
                var entity = new TuyaEntityDTO()
                {
                    Name = tuyaEntity.Name,
                    URL = tuyaEntity.Url,
                    Platform = tuyaEntity.Platform,
                    Icon = tuyaEntity.Icon,
                    AccessToken = tuyaEntity.AccessToken,
                    RefreshToken = tuyaEntity.RefreshToken,
                    Region = tuyaEntity.Region
                };
                teDTOs.Add(entity);
            }
            entities.AddRange(teDTOs);
            entities.AddRange(seDTOs);
            return Ok(entities);
        }

        // Új entitás hozzáadása a device-hoz
        [HttpPost("{deviceId}")]
        public async Task<ActionResult<EntityDTO>> AddEntity(string deviceId, AddEntityRequest model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userManager.FindByIdAsync(userId);
            var device = await context.Devices.FirstOrDefaultAsync(d => d.Id == Guid.Parse(deviceId.ToString()) && d.UserId == userId && d.Platform == model.Platform);

            if (device == null) return NotFound();

            if (model.Platform == "Tuya")
            {
                var entity = new TuyaEntity
                {
                    Url = model.URL,
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
                var DTO = new TuyaEntityDTO()
                {
                    Name = entity.Name,
                    URL = entity.Url,
                    Platform = entity.Platform,
                    Icon = entity.Icon,
                    AccessToken = entity.AccessToken,
                    RefreshToken = entity.RefreshToken,
                    Region = entity.Region
                };
                await context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetDeviceEntities), new { deviceId = device.Id }, DTO);
            }
            else if (model.Platform == "Somfy")
            {
                var entity = new SomfyEntity
                {
                    Url = model.URL,
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
                var DTO = new SomfyEntityDTO()
                {
                    Name = entity.Name,
                    URL = entity.Url,
                    Platform = entity.Platform,
                    Icon = entity.Icon,
                    BaseUrl = entity.BaseUrl,
                    GatewayPin = entity.GatewayPin,
                    SessionId = entity.SessionId,
                    Token = entity.Token
                };
                await context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetDeviceEntities), new { deviceId = device.Id }, DTO);
            }
            return BadRequest("Unsupported platform");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntity(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var tuyaEntity = await context.TuyaEntities.FirstOrDefaultAsync(e => e.Id == Guid.Parse(id) && e.UserId == userId);
            if (tuyaEntity == null)
            {
                var somfyEntity = await context.SomfyEntities.FirstOrDefaultAsync(e => e.Id == Guid.Parse(id) && e.UserId == userId);
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
