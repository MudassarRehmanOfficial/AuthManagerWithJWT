using Application.Contracts.Persistence.Helpers;
using Application.Contracts.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Helpers;
using Persistence.Repositories;

namespace Persistence
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
        {
            services.AddSingleton<IDbHelpers, DbHelpers>();
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }
    }
}
