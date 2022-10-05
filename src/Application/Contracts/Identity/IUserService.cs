using Application.DTOs;
using Application.Models.ClientInfo;
using Application.Models.Identity;

namespace Application.Contracts.Identity
{
    public interface IAuthService
    {
        Task<AuthenticateResponse?> AuthenticateUserAsync(AuthenticateRequest model);
        Task<RegistrationResponse?> CreateUserAsync(RegistrationRequest registerDto);
        Task<RevokeTokenResponse> RevokeTokensAsync(string userName);
        Task<RefreshTokenResponse> RefreshTokensAsync(string userName, TokenApiDto tokenApiDto);

    }
}