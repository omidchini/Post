using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using Serilog;

namespace Post.WebUI.Controllers
{
    public class AuthController : ApiControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(IConfiguration config, UserManager<IdentityUser> userManager)
        {
            _config = config;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginRequest user)
        {
            Log.Error("asdasdasdad");
            if (!ModelState.IsValid) {
                return BadRequest(new {
                    Result = false,
                    Errors = new List<string>() { "Invalid payload" }
                });
            }

            var existingUser = await _userManager.FindByEmailAsync(user.Email);
            if (existingUser == null)
            {
                return BadRequest(new
                {
                    Result = false,
                    Errors = new List<string>(){
                        "Invalid authentication request"
                    }
                });
            }

            var isCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);
            if (!isCorrect)
            {
                return BadRequest(new
                {
                    Result = false,
                    Errors = new List<string>() { "Invalid authentication request" }
                });
            }

            var jwtToken = GenerateJwtToken(existingUser);
            return Ok(new
            {
                Result = true,
                Token = jwtToken
            });

        }

        private string GenerateJwtToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_config["JwtConfig:Secret"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }

        public class UserLoginRequest
        {
            [Required]
            public string Email { get; set; }
            [Required]
            public string Password { get; set; }
        }
    }
}
