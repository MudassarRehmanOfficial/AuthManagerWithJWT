using System.Net.Http.Json;

using Application.Configurations;
using Application.Contracts.Identity;
using Application.Contracts.Persistence.Helpers;
using Application.Contracts.Persistence.Repositories;
using Application.DTOs;
using Application.Models.ClientInfo;
using Application.Models.Identity;

using Domain.Entities;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Persistence.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly IDbHelpers _dbHelpers;
        private readonly ISecurityHelpers _securityHelpers;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IPApi _ipApioptions;

        public UserRepository(IDbHelpers dbHelpers, ISecurityHelpers securityHelpers, IHttpContextAccessor contextAccessor, IOptions<IPApi> ipApioptions)
        {
            _dbHelpers = dbHelpers;
            _securityHelpers = securityHelpers;
            _contextAccessor = contextAccessor;
            _ipApioptions = ipApioptions.Value;
        }

        public async Task<Users> GetUserAsync(string userName)
        {
            return await _dbHelpers.GetRecordByStoreProcedureAsync<Users, object>("SP_GetUser", new { UserName = userName });
        }
        public async Task<UserDto?> AddUserAsync(UserDto model)
        {
            var isUserNameExist = await GetUserAsync(model.UserName!);
            if (isUserNameExist == null)
            {
                model.IPAddress ??= (await GetUserIPAsync())?.Query;
               return await _dbHelpers.InsertRecordByStoreProcedureAsync<UserDto, object>("SP_InsertUser", new { UserName = model.UserName, FirstName = model.FirstName, LastName = model.LastName, Device = model.Device, IPAddress = model.IPAddress, Password = model.Password });
            }
            else
            {
                return null;
            }
        }
        public async Task<List<Users>> GetAllUsersAsync()
        {
            return await _dbHelpers.GetAllByStoreProcedureAsync<Users>("SP_GetAllUsers");
        }
        public async Task<int> UpdateUsersAsync(UserDto userDto)
        {
            return await _dbHelpers.UpdateRecordByStoreProcedureAsync<object>("SP_UpdateUser", new { RefreshToken = userDto.RefreshToken, RefreshTokenExpiryTime = userDto.RefreshTokenExpiryTime });
        }
        public async Task<RequestOrigin?> GetUserIPAsync()
        {
            var requestUrl = _contextAccessor.HttpContext?.Request.Host;
            var client = new HttpClient();
            return await client.GetFromJsonAsync<RequestOrigin>($"{_ipApioptions.APIUrl}/{requestUrl}");
        }
    }
}
