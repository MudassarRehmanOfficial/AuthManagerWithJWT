using Application.Contracts.Identity;
using Application.Contracts.Persistence.Repositories;
using Application.DTOs;
using Application.Models.API;
using Application.Models.Identity;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;

        public UsersController(IAuthService authService, IUserRepository userRepository)
        {
            _authService = authService;
            _userRepository = userRepository;
        }

        [HttpPost("Signup")]
        public async Task<ActionResult> Register(RegistrationRequest registrationRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiValidationErrorResponse());
            }
            var response = await _authService.CreateUserAsync(registrationRequest);
            return response == null ? BadRequest(new ApiResponse(400, $"User '{registrationRequest.UserName}' already exists.")) : Ok(response);
        }
        [HttpPost("Authenticate")]
        public async Task<ActionResult> Login(AuthenticateRequest authenticateRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse(400));
            }
            var response = await _authService.AuthenticateUserAsync(authenticateRequest);
            return response == null ? BadRequest(new ApiResponse(400)) : Ok(response);
        }
        [HttpGet("GetAll"), Authorize]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _userRepository.GetAllUsersAsync());
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(TokenApiDto tokenApiDto)
        {
            if (tokenApiDto is null)
                return BadRequest("Invalid client request");

            var result = await _authService.RefreshTokensAsync(User.Identity?.Name!, tokenApiDto);
            return Ok(new RefreshTokenResponse()
            {
                Token = result.Token,
                RefreshToken = result.RefreshToken
            });
        }
        [HttpPost("revoke"), Authorize]
        public async Task<IActionResult> Revoke()
        {
            var result = await _authService.RevokeTokensAsync(User.Identity?.Name!);
            return result.Success ? Ok(new ApiResponse(200, "Tokens revoked successfully")) : BadRequest(new ApiResponse(401));
        }
    }
}
