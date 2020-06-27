using Identity.Application.Users;
using Identity.Infrastructure.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IUserRepository, LegacyUserRepository>();
            services.AddTransient<IUserRepository, LegacyUserRepository>();
            services.Configure<ConnectionStrings>(configuration.GetSection("ConnectionStrings"));

            return services;
        }
    }
}
