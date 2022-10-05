using System.Data.SqlTypes;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Application.Configurations;
using Application.Contracts.Identity;
using Application.Contracts.Persistence.Repositories;
using Application.DTOs;
using Application.Models.Identity;

using MapsterMapper;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace JWTIdentity.Services
{
    internal class AuthService : IAuthService
    {
        private readonly AppSettings _appSettings;
        private readonly IJSONWebTokenHelpers _jwtHelpers;
        private readonly IUserRepository _usersRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ISecurityHelpers _securityHelpers;
        private readonly IMapper _mapper;

        public AuthService(IOptions<AppSettings> appSettings, IJSONWebTokenHelpers jwtHelpers, IUserRepository usersRepository, IHttpContextAccessor contextAccessor, ISecurityHelpers securityHelpers, IMapper mapper)
        {
            _appSettings = appSettings.Value;
            _jwtHelpers = jwtHelpers;
            _usersRepository = usersRepository;
            _contextAccessor = contextAccessor;
            _securityHelpers = securityHelpers;
            _mapper = mapper;
        }
        public async Task<AuthenticateResponse?> AuthenticateUserAsync(AuthenticateRequest model)
        {
            var user = await _usersRepository.GetUserAsync(model.Username!);
            var verifyUserPassword = _securityHelpers.VerifyHashedPassword(user.Password!, model.Password!);
            if (verifyUserPassword)
            {
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, $"{user.UserName}"),
                    new Claim("UserName", $"{user.UserName}"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                // return null if user not found

                if (user == null) return null;

                // authentication successful so generate jwt token
                var securityToken = _jwtHelpers.GenerateJwtToken(_appSettings, authClaims);

                _contextAccessor.HttpContext?.Session.SetString("Token", new JwtSecurityTokenHandler().WriteToken(securityToken));

                var refreshToken = _jwtHelpers.GenerateRefreshToken();
                _ = int.TryParse(_appSettings.RefreshTokenValidityInMinutes, out int refreshTokenValidityInDays);
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidityInDays);
                var userDto = _mapper.Map<UserDto>(user);
                await _usersRepository.UpdateUsersAsync(userDto);
                return new AuthenticateResponse(userDto, new JwtSecurityTokenHandler().WriteToken(securityToken));
            }
            else
            {
                return null;
            }
        }
        public async Task<RegistrationResponse?> CreateUserAsync(RegistrationRequest registerDto)
        {
            var hashedPassword = _securityHelpers.PasswordHasher(registerDto.Password!);
            UserDto user = new UserDto() { UserName = registerDto.UserName, Password = hashedPassword, FirstName = registerDto.FirstName, LastName = registerDto.LastName, IPAddress = registerDto.IPAddress, Device = registerDto.Device };
            var result = await _usersRepository.AddUserAsync(user);
            return _mapper.Map<RegistrationResponse>(result!);
        }
        public async Task<RefreshTokenResponse> RefreshTokensAsync(string userName, TokenApiDto tokenApiDto)
        {
            var userDto = _mapper.Map<UserDto>(await _usersRepository.GetUserAsync(userName));
            var principal = _jwtHelpers.GetPrincipalFromExpiredToken(tokenApiDto.AccessToken, _appSettings);
            var authenticateRequest = _mapper.Map<AuthenticateRequest>(userDto);
            var user = await AuthenticateUserAsync(authenticateRequest);
            var refreshTokenResponse = _mapper.Map<RefreshTokenResponse>(user!);
            var newAccessToken = _jwtHelpers.GenerateJwtToken(_appSettings, principal.Claims.ToList());
            refreshTokenResponse.RefreshToken = _jwtHelpers.GenerateRefreshToken();
            refreshTokenResponse.Token = new JwtSecurityTokenHandler().WriteToken(newAccessToken);
            return refreshTokenResponse;
        }
        public async Task<RevokeTokenResponse> RevokeTokensAsync(string userName)
        {
            var user = await _usersRepository.GetUserAsync(userName);
            if (user is not null)
            {
                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = (DateTime)SqlDateTime.MinValue;
                var userDto = _mapper.Map<UserDto>(user);
                var result = await _usersRepository.UpdateUsersAsync(userDto);
                if (result != 0)
                {
                    return new RevokeTokenResponse() { Success = true };
                }
                else
                {
                    return new RevokeTokenResponse() { Success = false };
                }
            }
            else
            {
                return new RevokeTokenResponse() { Success = false };
            }
        }
    }
}
