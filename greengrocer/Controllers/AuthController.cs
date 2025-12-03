using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using greengrocer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace greengrocer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IConfiguration config,
            IUserService userService,
            ILogger<AuthController> logger)
        {
            _config = config;
            _userService = userService;
            _logger = logger;
        }

        public class LoginDto
        {
            [Required]
            public string Username { get; set; }

            [Required]
            public string Password { get; set; }
        }

        /// <summary>
        /// Kullanıcı adı / şifre ile JWT üretir.
        /// POST: /api/auth/token
        /// </summary>
        [HttpPost("token")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateToken([FromBody] LoginDto login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userService.ValidateUserAsync(login.Username, login.Password);
            if (user == null)
            {
                _logger.LogWarning("JWT login failed for username: {Username}", login.Username);
                return Unauthorized(new { error = "Invalid username or password." });
            }

            _logger.LogInformation("JWT login succeeded for user {Username}", user.UserName);

            var expires = DateTime.UtcNow.AddHours(2);

            // JWT claims
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("fullName", user.FullName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var keyBytes = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
            var key = new SymmetricSecurityKey(keyBytes);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expires,
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                token = tokenString,
                expiresAt = expires
            });
        }

        /// <summary>
        /// JWT ile korunan test endpoint'i.
        /// GET: /api/auth/test
        /// </summary>
        [HttpGet("test")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Test()
        {
            return Ok(new
            {
                message = "JWT is working",
                user = User.Identity?.Name
            });
        }
    }
}
