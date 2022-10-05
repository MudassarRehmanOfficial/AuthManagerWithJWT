using Mapster;
using MapsterMapper;

namespace Application
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            var config = new TypeAdapterConfig();
            // Or
            // var config = TypeAdapterConfig.GlobalSettings;
            services.AddSingleton(config);
            services.AddScoped<IMapper, Mapper>();
            return services;
        }
    }
}
