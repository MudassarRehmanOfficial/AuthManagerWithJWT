using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Application.Configurations;
using Domain.Entities;

namespace Application.Contracts.Identity
{
    public interface IJSONWebTokenHelpers
    {
        JwtSecurityToken GenerateJwtToken(AppSettings appSettings, List<Claim> authClaims);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string? token, AppSettings appSettings);
        string GenerateRefreshToken();
    }
}