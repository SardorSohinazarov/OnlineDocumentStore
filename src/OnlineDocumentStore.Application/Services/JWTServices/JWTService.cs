using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OnlineDocumentStore.Application.Abstractions;
using OnlineDocumentStore.Application.Models.Auth;
using OnlineDocumentStore.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineDocumentStore.Application.Services.JWTServices
{
    public class JWTService : IJWTService
    {
        private readonly IAppDbContext _appDbContext;
        private readonly JWTConfiguration _configuration;

        public JWTService(IAppDbContext appDbContext, IConfiguration configuration)
        {
            _configuration = configuration.GetSection("JWT").Get<JWTConfiguration>();
            _appDbContext = appDbContext;
        }

        public TokenDTO GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expireInMinutes = Convert.ToInt32(_configuration.ExpireInMinutes ?? "60");

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.FullName.ToString()));
            claims.Add(new Claim(ClaimTypes.Email, user.Email.ToString()));
            claims.Add(new Claim(ClaimTypes.MobilePhone, user.PhoneNumber.ToString()));

            var token = new JwtSecurityToken(
               issuer: _configuration.Issuer,
               audience: _configuration.Audience,
               claims: claims,
               expires: DateTime.Now.AddMinutes(expireInMinutes),
               signingCredentials: credentials);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new TokenDTO
            {
                AccessToken = accessToken,
                RefreshToken = user.RefreshToken,
                ExpireDate = user.ExpireDate
            };
        }

        public async ValueTask<TokenDTO> RefreshTokenAsync(TokenDTO tokenDTO)
        {
            var claims = await GetClaimsFromExpiredTokenAsync(tokenDTO.AccessToken);
            var id = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id.ToString() == id);

            if (user.RefreshToken != tokenDTO.RefreshToken)
                throw new Exception("Refresh token is not valid");

            if (user.ExpireDate <= DateTime.Now)
                throw new Exception("Refresh token has already been expired");

            //updating refresh token and expired date
            user.RefreshToken = Guid.NewGuid().ToString();
            user.ExpireDate = DateTime.Now.AddDays(7);

            await _appDbContext.SaveChangesAsync();

            return GenerateToken(user);
        }

        public async ValueTask<ClaimsPrincipal> GetClaimsFromExpiredTokenAsync(string token)
        {
            var validationParametrs = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = _configuration.Issuer,
                ValidateAudience = true,
                ValidAudience = _configuration.Audience,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Key)),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var claimsPrincipal = tokenHandler.ValidateToken(token, validationParametrs, out SecurityToken securityToken);
            var jwtsecurityToken = securityToken as JwtSecurityToken;

            if (jwtsecurityToken == null)
                throw new Exception("Invalid token");

            return claimsPrincipal;
        }
    }
}
