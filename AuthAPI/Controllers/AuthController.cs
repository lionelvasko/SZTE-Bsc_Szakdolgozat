using AuthAPI.Models;
using Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthAPI.Controllers
{
    /// <summary>
    /// Controller for handling authentication-related operations such as user registration and login.
    /// </summary>
    [Route("api/")]
    [ApiController]
    public class AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration) : ControllerBase
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly IConfiguration _config = configuration;

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="model">The registration request containing user details.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return BadRequest("Email is already registered.");
            }

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("User registered successfully.");
        }

        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="model">The login request containing user credentials.</param>
        /// <returns>An <see cref="IActionResult"/> containing the JWT token if login is successful.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Unauthorized("No account found.");
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (!result.Succeeded)
            {
                return Unauthorized("Invalid email or password.");
            }

            var token = GenerateJwtToken(user);
            return Ok(token);
        }

        /// <summary>
        /// Generates a JWT token for the specified user.
        /// </summary>
        /// <param name="user">The user for whom the token is generated.</param>
        /// <returns>A JWT token as a string.</returns>
        private string? GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var keySection = _config.GetSection("Jwt:Key");
            var keyValue = keySection?.Value;

            if (string.IsNullOrEmpty(keyValue))
            {
                throw new InvalidOperationException("JWT key is not configured properly.");
            }

            var key = Encoding.ASCII.GetBytes(keyValue);

            if (user.Email is null || user.FirstName is null || user.LastName is null)
            {
                return null;
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
