using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace dotnetcore7_webapi_authentication.Middlewares
{
    public static class JwtCookieMiddleware
    {
        public static void UseJwtCookieMiddleware(this IApplicationBuilder app, byte[] key)
        {
            app.Use( (context, next) =>
            {
                string? accessToken = context.Request.Cookies["access-token"];
                if (string.IsNullOrEmpty(accessToken))
                {
                    throw new Exception("No access token");
                }
                var parameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                try
                {
                    var claimsPrincipal = new JwtSecurityTokenHandler().ValidateToken(accessToken, parameters, out var validatedToken);
                }
                catch
                {
                    throw new Exception("Invalid token");
                }

                return next();
            });
        }
    }
}