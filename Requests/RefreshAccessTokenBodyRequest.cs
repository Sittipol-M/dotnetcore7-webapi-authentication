namespace dotnetcore7_webapi_authentication.Requests
{
    public class RefreshAccessTokenBodyRequest
    {
        public required string AccessToken { get; set; }
    }
}