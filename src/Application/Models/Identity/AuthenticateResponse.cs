using Application.DTOs;

namespace Application.Models.Identity
{
    public class AuthenticateResponse
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Token { get; set; }


        public AuthenticateResponse(UserDto user, string token)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            Token = token;
        }
    }
}
