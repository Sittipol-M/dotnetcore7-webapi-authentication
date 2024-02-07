using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using dotnetcore7_webapi_authentication.Data;
using dotnetcore7_webapi_authentication.Requests;
using dotnetcore7_webapi_authentication.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using User = dotnetcore7_webapi_authentication.Models.User;

namespace dotnetcore7_webapi_authentication.Services
{
    public interface IAuthService
    {
        public Task<LoginResponse> Login(LoginBodyRequest bodyRequest);
        public Task Register(RegisterBodyRequest bodyRequest);
        public Task<RefreshAccessTokenBodyResponse> RefreshAccessToken(string? accessToken, string? refreshToken);
    }
    public class AuthService : IAuthService
    {
        private readonly Dotnetcore7WebapiAuthenticationDbContext _context;
        private readonly IConfiguration _configuration;
        public AuthService(Dotnetcore7WebapiAuthenticationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        private string GenerateAccessToken(User user)
        {
            var claims = new List<Claim>{
                new(ClaimTypes.Name,user.Username),
                new(ClaimTypes.Role,user.Role)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(1),
                signingCredentials: credentials
            );
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            return accessToken;
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            RandomNumberGenerator.Create().GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        public async Task<LoginResponse> Login(LoginBodyRequest bodyRequest)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username.Equals(bodyRequest.Username));
            if (user is null)
            {
                throw new Exception("invalid username");
            }

            bool isPasswordVerify = BCrypt.Net.BCrypt.Verify(bodyRequest.Password, user.Password);
            if (!isPasswordVerify)
            {
                throw new Exception("invalid password");
            }

            var accessToken = GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpireAt = DateTime.UtcNow.AddDays(1);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            var response = new LoginResponse()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                User = new() { Username = bodyRequest.Username, Role = user.Role },
            };
            return response;
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

        public async Task<RefreshAccessTokenBodyResponse> RefreshAccessToken(string? accessToken, string? refreshToken)
        {
            if (refreshToken is null)
            {
                throw new Exception("not enough tokens");
            }

            TokenValidationParameters validationParameters = new()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"])),
                ValidateLifetime = false
            };

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(accessToken, validationParameters, out SecurityToken validatedToken);

            string? username = principal?.Identity?.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username.Equals(username));
            if (user is null)
            {
                throw new Exception("invalid refresh token");
            }
            if (!refreshToken.Equals(user.RefreshToken))
            {
                throw new Exception("invalid refresh token");
            }
            if (user.RefreshTokenExpireAt < DateTime.UtcNow)
            {
                throw new Exception("token is expire");
            }

            var newAccessToken = GenerateAccessToken(user);
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            var response = new RefreshAccessTokenBodyResponse()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                User = new() { Username = user.Username, Role = user.Role },
            };

            return response;
        }
    }
}