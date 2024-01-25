
namespace dotnetcore7_webapi_authentication.Responses
{
    public class LoginResponse
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}