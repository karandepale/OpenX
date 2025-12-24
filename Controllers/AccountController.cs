using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenX.Data;
using OpenX.DTO_s;
using OpenX.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OpenX.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AccountController(AppDbContext context, IConfiguration config) : ControllerBase
    {
        private readonly AppDbContext _context = context;
        private readonly IConfiguration _config = config;

      
         
        [HttpPost("signup")]
        public IActionResult Signup([FromBody] SignupDto request)
        {
            try
            {
                bool userExists = _context.Users.Any(u => u.UserName == request.UserName);
                if (userExists)
                    return BadRequest("Username already exists");

                var user = new UserDetails
                {
                    UserId = Guid.NewGuid(),
                    UserName = request.UserName,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    CreatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);
                _context.SaveChanges();

                var token = GenerateJwtToken(user);

                return Ok(new
                {
                    message = "User registered successfully",
                    token,
                    expiresIn = _config["Jwt:ExpireMinutes"]
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error AccountController: inSignup(): {ex.Message}, {ex.StackTrace}");
                return null;
            }
         
        }





        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDto request)
        {
            try
            {
            var user = _context.Users
                .FirstOrDefault(u => u.UserName == request.UserName);

            if (user == null)
                return Unauthorized("Invalid username or password");

            bool isPasswordValid =
                BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

            if (!isPasswordValid)
                return Unauthorized("Invalid username or password");

            var token = GenerateJwtToken(user);

            return Ok(new
            {
                message = "User Logged in successfully",
                token,
                expiresIn = _config["Jwt:ExpireMinutes"]
            });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error AccountController: Login(): {ex.Message}, {ex.StackTrace}");
                return null;
            }
        }



        private string GenerateJwtToken(UserDetails user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
            );

            var creds = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(_config["Jwt:ExpireMinutes"])
                ),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
