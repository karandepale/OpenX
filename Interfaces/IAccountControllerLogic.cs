using Azure.Core;
using OpenX.DTO_s;
using OpenX.Models;

namespace OpenX.Interfaces
{
    public interface IAccountControllerLogic
    {
        Task<OpenXResponse> Signup(SignupDto request);
        Task<OpenXResponse> Login(LoginRequestDto request);
    }
}
