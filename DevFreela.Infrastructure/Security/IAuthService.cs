using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DevFreela.Infrastructure.Security
{
    public interface IAuthService
    {
        string ComputeHash(string password);
        string GenerateToken(string email, string role);
    }

    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string ComputeHash(string password)
        {
            throw new NotImplementedException();
        }

        public string GenerateToken(string email, string role)
        {
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = new SymmetricSecurityKey( Encoding.UTF8.GetBytes(_configuration["Jwt:Issuer"]));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim("username",email),
                new Claim(ClaimTypes.Role,role)
            };

            var token = new JwtSecurityToken(issuer, audience, claims, null, DateTime.Now.AddHours(2), credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
