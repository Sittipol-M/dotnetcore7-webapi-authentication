namespace dotnetcore7_webapi_authentication.Requests
{
    public class RefreshAccessTokenBodyRequest
    {
        public required string RefreshToken { get; set; }
    }
}