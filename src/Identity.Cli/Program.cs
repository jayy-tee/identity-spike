using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Identity.Cli;
using Identity.Application.Users;
using Identity.Infrastructure;
using Identity.Infrastructure.Config;

namespace Identity.Cli
{
    class Program
    {
        static void Main(string[] args)
        {

            var serviceProvider = ConfigureServices().BuildServiceProvider();
            serviceProvider.GetService<IdentityCli>().Run();
        }

        private static IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();
            var configuration = ConfigurationSingleton.Instance;
            services.Configure<ConnectionStrings>(configuration.GetSection("ConnectionStrings"));
            services.AddLogging(configure => configure.AddConsole());
            services.AddScoped<IUserRepository, LegacyUserRepository>();
            services.AddScoped<IUserRepository, ModernUserRepository>();
            services.AddTransient<IUserFacade, UserFacade>();
            services.AddTransient<IdentityCli, IdentityCli>();
            return services;
        }
    }
}
