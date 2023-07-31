using JobManagementApi.Models.Connections;
using JobManagementApi.Models.DTOS;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JobManagementApi.JWT
{
    public class JWTGenerator : IJWTGenerator
    {
        private IConfiguration _configuration;
        public JWTGenerator(IConfiguration configuration)
        {
         _configuration = configuration;   
        }
        public string GenerateToken(Claim[] claims)
        {

            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("ajlsfhlkajhdsakljhfkljhasjklhgasjdghsklhljhl");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
