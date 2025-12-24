using Microsoft.IdentityModel.Tokens;
using OpenX.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OpenX.HelperClasses
{
    public class JwtTokenHelper(IConfiguration appConfig)
    {
        private readonly IConfiguration appConfig = appConfig;
      
        public string GenerateJwtToken(UserDetails user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(appConfig["Jwt:Key"]!)
            );

            var creds = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: appConfig["Jwt:Issuer"],
                audience: appConfig["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(appConfig["Jwt:ExpireMinutes"])
                ),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
