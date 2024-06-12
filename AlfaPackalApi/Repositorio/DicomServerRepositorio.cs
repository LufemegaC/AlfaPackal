using AlfaPackalApi.Datos;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Api_PACsServer.Repositorio
{
    public class DicomServerRepositorio
    {
        private readonly ApplicationDbContext _db;
        private readonly string _secretKey;

        public DicomServerRepositorio(ApplicationDbContext db, string secretKey)
        {
            _db = db;
            _secretKey = secretKey;
        }

        public async Task<string> GenerateDicomServerToken(string ipAddress)
        {
            // Posicionar el servidor dicom 
            var dicomServer = await _db.DicomServers.FirstOrDefaultAsync(w => w.IP == ipAddress);
            if (dicomServer == null)
            {
                throw new UnauthorizedAccessException("IP not in server list.");
            }
            // Validar que exista en la WhiteList y que este activo
            var whitelistedIp = await _db.WhitelistedIPs.FirstOrDefaultAsync(w => w.DicomServerId == dicomServer.Id && w.IsActive);
            if (whitelistedIp == null)
            {
                throw new UnauthorizedAccessException("IP not whitelisted.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim("IP", ipAddress),
                new Claim("InstitutionId", whitelistedIp.DicomServerId.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<string> RenewTokenIfNeeded(string currentToken, string ipAddress)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var principal = tokenHandler.ValidateToken(currentToken, validationParameters, out var validatedToken);
                var exp = long.Parse(principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp).Value);
                var expirationTime = DateTimeOffset.FromUnixTimeSeconds(exp).UtcDateTime;

                // Check if the token is about to expire in the next 30 minutes
                if (expirationTime < DateTime.UtcNow.AddMinutes(30))
                {
                    return await GenerateDicomServerToken(ipAddress);
                }
                return currentToken;
            }
            catch (Exception)
            {
                // Token is invalid or expired, generate a new one
                return await GenerateDicomServerToken(ipAddress);
            }
        }
    }
}
