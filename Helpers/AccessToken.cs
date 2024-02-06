using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace dotnetcore7_webapi_authentication.Helpers
{
    public class AccessToken
    {
        public void Validate(string token, TokenValidationParameters parameters)
        {
            try
            {
                ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(token, parameters, out SecurityToken validatedToken);
            }
            catch
            {
                throw new Exception("Invalid token");
            }

        }
    }
}