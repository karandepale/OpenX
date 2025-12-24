using Azure.Core;
using OpenX.AuthService.DTO_s;
using OpenX.AuthService.Models;

namespace OpenX.AuthService.Interfaces
{
    public interface IAccountControllerLogic
    {
        Task<OpenXResponse> Signup(SignupDto request);
        Task<OpenXResponse> Login(LoginRequestDto request);
    }
}
