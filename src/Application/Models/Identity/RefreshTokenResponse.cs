using Application.DTOs;

namespace Application.Models.Identity
{
    public class RefreshTokenResponse
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
    }
}
