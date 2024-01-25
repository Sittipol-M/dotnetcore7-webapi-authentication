namespace dotnetcore7_webapi_authentication.Requests
{
    public class RegisterBodyRequest
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
    }
}