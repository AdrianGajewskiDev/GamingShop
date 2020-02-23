using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GamingShop.Web.API
{
    public class JWTToken
    {
        private readonly ApplicationOptions _options;

        public JWTToken(IOptions<ApplicationOptions> options)
        {
            _options = options.Value;
        }

        public string CreateToken(string name, string value, double expires)
        {
            var claim = new Claim[] 
            {
                new Claim(name, value)
            };

            return CreateToken(claim, expires);
        }
        public string CreateToken(Claim[] claims, double expires)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(expires),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret_Key)), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);

            return token;
        }
    }
}
