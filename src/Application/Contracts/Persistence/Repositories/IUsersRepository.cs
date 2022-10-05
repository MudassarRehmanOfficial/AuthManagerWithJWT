using Application.DTOs;
using Application.Models.Identity;
using Domain.Entities;

namespace Application.Contracts.Persistence.Repositories
{
    public interface IUserRepository
    {
        Task<UserDto?> AddUserAsync(UserDto model);
        Task<Users> GetUserAsync(string userName);
        Task<List<Users>> GetAllUsersAsync();
        Task<int> UpdateUsersAsync(UserDto userDto);
    }
}