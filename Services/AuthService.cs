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
        public Task<RefreshAccessTokenBodyResponse> RefreshAccessToken(RefreshAccessTokenBodyRequest bodyRequest);
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
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: credentials
            );
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            return accessToken;
        }

        private string GenerateRefreshToken(User user)
        {
            var claims = new List<Claim>{
                new(ClaimTypes.Name,user.Username),
                new(ClaimTypes.Role,user.Role)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials
            );
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            return accessToken;
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
            var refreshToken = GenerateRefreshToken(user);

            user.RefreshToken = refreshToken;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            var response = new LoginResponse()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                User = new() { Username = bodyRequest.Username },
                Role = user.Role
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

        public async Task<RefreshAccessTokenBodyResponse> RefreshAccessToken(RefreshAccessTokenBodyRequest bodyRequest)
        {
            if (bodyRequest.RefreshToken is null)
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
            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(bodyRequest.RefreshToken, validationParameters, out SecurityToken validatedToken);

            string? username = principal?.Identity?.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username.Equals(username));
            if (user is null)
            {
                throw new Exception("invalid access token ");
            }
            if (!bodyRequest.RefreshToken.Equals(user.RefreshToken))
            {
                throw new Exception("invalid refresh token");
            }

            var newAccessToken = GenerateAccessToken(user);
            var newRefreshToken = GenerateRefreshToken(user);

            user.RefreshToken = newRefreshToken;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            var response = new RefreshAccessTokenBodyResponse()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };

            return response;
        }
    }
}