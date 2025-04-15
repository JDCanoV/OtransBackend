using Microsoft.IdentityModel.Tokens;
using OtransBackend.Dtos;
using OtransBackend.Repositories.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace OtransBackend.Utilities
{
    public class JWTUtility
    {
        public static ResponseLoginDto? GenTokenkey(ResponseLoginDto userToken, JwtSettingsDto jwtSettings)
        {
            try
            {
                if (userToken == null) throw new ArgumentException(nameof(userToken));
                // Get secret key
                var key = System.Text.Encoding.ASCII.GetBytes(jwtSettings.IssuerSigningKey);
                DateTime expireTime = DateTime.Now;
                if (jwtSettings.FlagExpirationTimeHours)
                {
                    expireTime = DateTime.Now.AddHours(jwtSettings.ExpirationTimeHours);
                }
                else
                {
                    if (jwtSettings.FlagExpirationTimeMinutes)
                    {
                        expireTime = DateTime.Now.AddMinutes(jwtSettings.ExpirationTimeMinutes);
                    }
                    else
                    {
                        return null;
                    }
                }
                IEnumerable<Claim> claims = new Claim[] {
                    new Claim("TiempoExpiracion", expireTime.ToString("yyyy-MM-dd HH:mm:ss")),
                    new Claim("Usuario", "Oblaanc")
                };
                var JWToken = new JwtSecurityToken(
                    issuer: jwtSettings.ValidIssuer,
                    audience: jwtSettings.ValidAudience,
                    claims: claims,
                    notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                    expires: new DateTimeOffset(expireTime).DateTime,
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256));
                userToken.Token = new JwtSecurityTokenHandler().WriteToken(JWToken);
                userToken.TiempoExpiracion = expireTime;
                return userToken;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
