
namespace dotnetcore7_webapi_authentication.Responses
{
    public class RefreshAccessTokenBodyResponse
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
        public required User User { get; set; }
        public required string Role { get; set; }
    }
}