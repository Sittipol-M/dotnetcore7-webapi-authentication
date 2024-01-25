namespace dotnetcore7_webapi_authentication.Requests
{
    public class LoginBodyRequest
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}