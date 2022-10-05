using System.ComponentModel.DataAnnotations;

namespace Application.Models.Identity
{
    public class AuthenticateRequest
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }

        [Required]
        public string? IPAddress { get; set; }

        [Required]
        public string? Device { get; set; }
    }
}
