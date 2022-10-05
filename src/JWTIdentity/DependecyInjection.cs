using Application.Contracts.Identity;
using Application.DTOs;

using Domain.Entities;

using JWTIdentity.Helpers;
using JWTIdentity.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace JWTIdentity
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddTransient<IJSONWebTokenHelpers, JSONWebTokenHelpers>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddSingleton(typeof(ISecurityHelpers), typeof(SecurityHelpers));
            return services;
        }
    }
}
