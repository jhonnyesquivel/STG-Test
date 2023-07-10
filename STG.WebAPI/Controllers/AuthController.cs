using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using STG.Core.Services;
using STG.WebAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace STG.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public AuthController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userService.GetUser(model.Username, model.Password);
            if (!_userService.ValidateCredentials(user, model.Password))
            {
                return Unauthorized();
            }

            var token = GenerateJwtToken(model.Username, user.UserId);

            return Ok(new { token });
        }

        private string GenerateJwtToken(string username, int userId)
        {
            var secretKey = _configuration.GetValue<string>("JwtSettings:SecretKey");
            var issuer = _configuration.GetValue<string>("JwtSettings:Issuer");
            var audience = _configuration.GetValue<string>("JwtSettings:Audience");

            var key = Encoding.ASCII.GetBytes(secretKey!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials
             (new SymmetricSecurityKey(key),
             SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            var stringToken = tokenHandler.WriteToken(token);
            return stringToken;
        }
    }
}