using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OpenX.Data;
using OpenX.DTO_s;
using OpenX.HelperClasses;
using OpenX.Interfaces;
using OpenX.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OpenX.BusinessLogic
{
    public class AccountControllerLogic(AppDbContext context, IConfiguration appConfig, JwtTokenHelper jwtHelper) : IAccountControllerLogic
    {
        private readonly AppDbContext context = context;
        private readonly IConfiguration appConfig = appConfig;
        private readonly JwtTokenHelper jwtHelper = jwtHelper;

        public async Task<OpenXResponse> Signup(SignupDto request)
        {
            try
            {
                bool userExists = await context.Users.AnyAsync(u => u.UserName == request.UserName);
                if (userExists)
                {
                    return new OpenXResponse
                    {
                        Success = false,
                        Message = "Username already exists",
                        JwtToken = string.Empty
                    };
                }
                
                var user = new UserDetails
                {
                    UserId = Guid.NewGuid(),
                    UserName = request.UserName,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    CreatedAt = DateTime.UtcNow
                };

                context.Users.Add(user);
                await context.SaveChangesAsync();

                var token = jwtHelper.GenerateJwtToken(user);
                return new OpenXResponse
                {
                    Success = true,
                    Message = "Signup logic executed successfully",
                    JwtToken = token 
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AccountControllerLogic: Signup(): {ex.Message}, {ex.StackTrace}");
                throw; 
            }
        }

        public async Task<OpenXResponse> Login(LoginRequestDto request)
        {
            try
            {
                var user = await context.Users.FirstOrDefaultAsync(u => u.UserName == request.UserName);

                if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                {
                    return new OpenXResponse
                    {
                        Success = false,
                        Message = "Invalid username or password",
                        JwtToken = string.Empty
                    };
                }

                var token = jwtHelper.GenerateJwtToken(user);
                return new OpenXResponse
                {
                    Success = true,
                    Message = "Login logic executed successfully",
                    JwtToken = token
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AccountControllerLogic: Login(): {ex.Message}, {ex.StackTrace}");
                throw; 
            }
        }

         
     



    }
}
