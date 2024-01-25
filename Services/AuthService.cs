using dotnetcore7_webapi_authentication.Requests;
using dotnetcore7_webapi_authentication.Responses;

namespace dotnetcore7_webapi_authentication.Services
{
    public interface IAuthService {
        public LoginResponse Login(LoginBodyRequest bodyRequest);
        public RegisterResponse Register(RegisterBodyRequest bodyRequest);
    }
    public class AuthService : IAuthService
    {
        public LoginResponse Login(LoginBodyRequest bodyRequest)
        {
            throw new NotImplementedException();
        }

        public RegisterResponse Register(RegisterBodyRequest bodyRequest)
        {
            throw new NotImplementedException();
        }
    }
}