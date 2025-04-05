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
    public class EntitiesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public EntitiesController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // A device-hoz tartozó entitások lekérésére
        [HttpGet("Device/{deviceId}")]
        public async Task<ActionResult<IEnumerable<Entity>>> GetDeviceEntities(int deviceId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var device = await _context.Devices.Include(d => d.Entities)
                .FirstOrDefaultAsync(d => d.Id == deviceId && d.UserId == userId);

            if (device == null) return NotFound();

            return Ok(device.Entities);
        }

        //A userhez tartozó device-ok entitásainak lekérésére
        [HttpGet("UserEntities")]
        public async Task<ActionResult<IEnumerable<Entity>>> GetUsersEntities()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var devices = await _context.Devices.Include(d => d.Entities)
                .Where(d => d.UserId == userId)
                .ToListAsync();
            if (devices == null) return NotFound();
            var entities = devices.SelectMany(d => d.Entities).ToList();
            return Ok(entities);
        }

        //Új entitás hozzáadása a device-hoz
        [HttpPost]
        public async Task<ActionResult<Entity>> AddEntity(int deviceId, AddEntity model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            var device = await _context.Devices.FirstOrDefaultAsync(d => d.Id == deviceId && d.UserId == userId);
            if (device == null) return NotFound();

            var entity = new Entity
            {
                URL = model.URL,
                Name = model.Name,
                Platform = model.Platform,
                Icon = model.Icon,
                DeviceId = device.Id,
                UserId = user.Id
            };

            _context.Entities.Add(entity);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDeviceEntities), new { deviceId = device.Id }, entity);
        }



        // 5. Entitás törlés
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntity(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var entity = await _context.Entities.FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);
            if (entity == null) return NotFound();

            _context.Entities.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
