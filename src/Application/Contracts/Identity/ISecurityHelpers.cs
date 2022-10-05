using Microsoft.AspNetCore.Identity;

namespace Application.Contracts.Identity
{
    public interface ISecurityHelpers
    {
        string PasswordHasher(string password);
        bool VerifyHashedPassword(string hashedPassword, string password);
    }
}