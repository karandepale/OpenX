using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OpenX.AuthService.Data;
using OpenX.AuthService.DataWrapper;
using OpenX.AuthService.DTO_s;
using OpenX.AuthService.HelperClasses;
using OpenX.AuthService.Interfaces;
using OpenX.AuthService.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OpenX.AuthService.BusinessLogic
{
    public class AccountControllerLogic(DataBaseOparations dataBaseOparations, IConfiguration appConfig, JwtTokenHelper jwtHelper, TimeProvider timeProvider) : IAccountControllerLogic
    {
        private readonly DataBaseOparations dataBaseOparations = dataBaseOparations;
        private readonly IConfiguration appConfig = appConfig;
        private readonly JwtTokenHelper jwtHelper = jwtHelper;
        private readonly TimeProvider timeProvider = timeProvider;

        public async Task<OpenXResponse> Signup(SignupDto request)
        { 
            try
            {
                bool userExists = await dataBaseOparations.UserExists(request.UserName);
                if (userExists)
                {
                    return new OpenXResponse
                    {
                        Success = false,
                        Message = "User already exists",
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
              
                await dataBaseOparations.CreateUser(user);
                var token = jwtHelper.GenerateJwtToken(user);
               
                return new OpenXResponse
                {
                    Success = true,
                    Message = "User registered successfully",
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
                var user = await dataBaseOparations.GetUserByUsername(request.UserName);

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
                    Message = "User Logged in successfully",
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
