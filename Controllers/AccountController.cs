using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenX.Data;
using OpenX.DTO_s;
using OpenX.Interfaces;
using OpenX.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OpenX.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AccountController(IConfiguration appConfig, IAccountControllerLogic accountControllerLogic) : ControllerBase
    {
        private readonly IConfiguration appConfig = appConfig;
        private readonly IAccountControllerLogic accountControllerLogic = accountControllerLogic;


        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupDto request)
        {
            try
            {
                var signupRes =  await accountControllerLogic.Signup(request);
                if (signupRes.Success)
                { 
                    return Ok(new
                    {
                        message = "User registered successfully",
                        token = signupRes.JwtToken,
                        expiresIn = appConfig["Jwt:ExpireMinutes"]
                    });
                }    
                else
                { 
                    return BadRequest(signupRes.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error AccountController: inSignup(): {ex.Message}, {ex.StackTrace}");
                return BadRequest(ex.Message);
            }

        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            try
            {
                var loginRes = await accountControllerLogic.Login(request);

                if (loginRes.Success)
                {
                    return Ok(new
                    {
                        message = "User Logged in successfully",
                        token = loginRes.JwtToken,
                        expiresIn = appConfig["Jwt:ExpireMinutes"]
                    });
                }
                else
                {
                    return BadRequest(loginRes.Message);
                }   
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error AccountController: Login(): {ex.Message}, {ex.StackTrace}");
                return BadRequest(ex.Message);
            }
        }



      

    }
}
