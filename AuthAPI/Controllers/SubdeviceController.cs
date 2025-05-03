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
    /// Controller for managing subdevices associated with a main device or user.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="SubDeviceController"/> class.
    /// </remarks>
    /// <param name="context">The database context.</param>
    /// <param name="userManager">The user manager for handling user-related operations.</param>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SubDeviceController(AppDbContext context, UserManager<User> userManager) : ControllerBase
    {
        private readonly AppDbContext context = context;
        private readonly UserManager<User> userManager = userManager;

        /// <summary>
        /// Retrieves the subdevices associated with a specific main device.
        /// </summary>
        /// <param name="maindeviceId">The ID of the main device.</param>
        /// <returns>A list of subdevice DTOs associated with the specified main device.</returns>
        [HttpGet("{maindeviceId}")]
        public async Task<ActionResult<IEnumerable<EntityDTO>>> GetDeviceSubdevices(string maindeviceId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var device = await context.Devices.FirstOrDefaultAsync(d => d.Id == Guid.Parse(maindeviceId) && d.UserId == userId);
            if (device == null) return NotFound();
            if (device.Platform == "Tuya")
            {
                var tuyaEntities = await context.TuyaEntities
                    .Where(e => e.DeviceId == Guid.Parse(maindeviceId) && e.UserId == userId)
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
                    .Where(e => e.DeviceId == Guid.Parse(maindeviceId) && e.UserId == userId)
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
                        CloudPasswordHashed = somfyEntity.CloudPasswordHashed,
                        CloudUsername = somfyEntity.CloudUsername
                    };
                    DTOs.Add(entityDTO);
                }
                return Ok(DTOs);
            }
            return NotFound();
        }

        /// <summary>
        /// Retrieves all subdevices associated with the authenticated user.
        /// </summary>
        /// <returns>A list of subdevice DTOs associated with the authenticated user.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EntityDTO>>> GetUsersSubdevices()
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
                    CloudPasswordHashed = somfyEntity.CloudPasswordHashed,
                    CloudUsername = somfyEntity.CloudUsername
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

        /// <summary>
        /// Adds a new subdevice to the specified main device.
        /// </summary>
        /// <param name="maindeviceId">The ID of the main device to which the subdevice will be added.</param>
        /// <param name="model">The details of the subdevice to be added.</param>
        /// <returns>The added subdevice as a DTO.</returns>
        [HttpPost("{maindeviceId}")]
        public async Task<ActionResult<EntityDTO>> AddSubdevice(string maindeviceId, AddEntityRequest model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is null or empty.");
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var device = await context.Devices.FirstOrDefaultAsync(d => d.Id == Guid.Parse(maindeviceId.ToString()) && d.UserId == userId && d.Platform == model.Platform);

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
                return CreatedAtAction(nameof(AddSubdevice), new { deviceId = device.Id }, DTO);
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
                    CloudUsername = model.CloudUsername,
                    CloudPasswordHashed = model.CloudPasswordHashed
                };
                context.SomfyEntities.Add(entity);
                var DTO = new SomfyEntityDTO()
                {
                    Name = entity.Name,
                    URL = entity.Url,
                    Platform = entity.Platform,
                    Icon = entity.Icon,
                    BaseUrl = entity.BaseUrl,
                    CloudUsername = entity.CloudUsername,
                    CloudPasswordHashed = entity.CloudPasswordHashed
                };
                await context.SaveChangesAsync();
                return CreatedAtAction(nameof(AddSubdevice), new { deviceId = device.Id }, DTO);
            }
            return BadRequest("Unsupported platform");
        }

        /// <summary>
        /// Deletes a subdevice entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the subdevice entity to delete.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
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
