using AuthAPI.Data;
using AuthAPI.DTOs;
using AuthAPI.Models;
using Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthAPI.Controllers
{
    /// <summary>
    /// Controller for managing user-related operations such as retrieving user details, updating user information, and deleting users.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        /// <param name="userManager">The user manager for handling user-related operations.</param>
        public UserController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Retrieves the details of the currently authenticated user.
        /// </summary>
        /// <returns>A <see cref="UserDTO"/> containing user details.</returns>
        [HttpGet]
        public async Task<ActionResult<UserDTO>> GetUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();
            var userDto = new UserDTO
            {
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName ?? string.Empty,
                LastName = user.LastName ?? string.Empty
            };
            return Ok(userDto);
        }

        /// <summary>
        /// Updates the password of the currently authenticated user.
        /// </summary>
        /// <param name="model">The request model containing the current and new passwords.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPut("Password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequest model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Ok("Password updated successfully.");
        }

        /// <summary>
        /// Updates the name of the currently authenticated user.
        /// </summary>
        /// <param name="model">The request model containing the new first and last names.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPut("Name")]
        public async Task<IActionResult> UpdateName([FromBody] UpdateNameRequest model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Ok("User name updated successfully.");
        }

        /// <summary>
        /// Deletes the currently authenticated user.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Ok("User deleted successfully.");
        }
    }
}
