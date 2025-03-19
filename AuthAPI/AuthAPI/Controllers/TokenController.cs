using AuthAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthAPI.Controllers
{
    [Route("authapi/tokens/")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public TokenController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        private async Task<User?> GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return null;

            return await _userManager.FindByIdAsync(userId);
        }

        [Authorize]
        [HttpGet("somfy")]
        public async Task<IActionResult> GetSomfyToken()
        {
            var user = await GetCurrentUser();
            if (user == null) return Unauthorized("User not found.");

            return Ok(new { AccesToken = user.SomfyToken, Url = user.SomfyUrl});
        }

        [Authorize]
        [HttpPost("somfy")]
        public async Task<IActionResult> SetSomfyToken([FromBody] TokenUpdateModel model)
        {
            var user = await GetCurrentUser();
            if (user == null) return Unauthorized("User not found.");

            user.SomfyToken = model.Token;
            user.SomfyUrl = model.Url;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok("SomfyToken updated successfully.");
        }

        [Authorize]
        [HttpGet("tuya")]
        public async Task<IActionResult> GetTuyaToken()
        {
            var user = await GetCurrentUser();
            if (user == null) return Unauthorized("User not found.");

            return Ok(new { AccesToken = user.TuyaToken , RefreshToken=user.TuyaRefreshToken, Region = user.TuyaRegion});
        }

        [Authorize]
        [HttpPost("tuya")]
        public async Task<IActionResult> SetTuyaToken([FromBody] TokenUpdateModel model)
        {
            var user = await GetCurrentUser();
            if (user == null) return Unauthorized("User not found.");

            user.TuyaToken = model.Token;
            user.TuyaRefreshToken = model.RefreshToken;
            user.TuyaRegion = model.Region;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok("TuyaToken updated successfully.");
        }
    }
}
