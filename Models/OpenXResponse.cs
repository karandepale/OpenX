namespace OpenX.AuthService.Models
{
    public class OpenXResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string JwtToken { get; set; } = string.Empty;

    }
}
