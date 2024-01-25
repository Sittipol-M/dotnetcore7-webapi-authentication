using BCrypt.Net;
using dotnetcore7_webapi_authentication.Data;
using dotnetcore7_webapi_authentication.Models;
using dotnetcore7_webapi_authentication.Requests;
using dotnetcore7_webapi_authentication.Responses;

namespace dotnetcore7_webapi_authentication.Services
{
    public interface IAuthService
    {
        public LoginResponse Login(LoginBodyRequest bodyRequest);
        public Task Register(RegisterBodyRequest bodyRequest);
    }
    public class AuthService : IAuthService
    {
        private readonly Dotnetcore7WebapiAuthenticationDbContext _context;
        public AuthService(Dotnetcore7WebapiAuthenticationDbContext context)
        {
            _context = context;
        }
        public LoginResponse Login(LoginBodyRequest bodyRequest)
        {
            throw new NotImplementedException();
        }

        public async Task Register(RegisterBodyRequest bodyRequest)
        {
            if (!bodyRequest.Password.Equals(bodyRequest.ConfirmPassword))
            {
                throw new Exception("password is not equal");
            }
            var hashPassword = BCrypt.Net.BCrypt.HashPassword(bodyRequest.Password);
            var newUser = new User()
            {
                Username = bodyRequest.Username,
                Password = hashPassword,
            };
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
        }
    }
}