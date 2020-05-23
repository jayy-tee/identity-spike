using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Identity.Cli;
using Identity.Application.Users;
using Identity.Infrastructure;

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
            services.AddLogging(configure => configure.AddConsole());
            services.AddScoped<IUserRepository, FakeUserRepository>();
            services.AddScoped<IUserRepository, ModernUserRepository>();
            services.AddTransient<IUserFacade, UserFacade>();
            services.AddTransient<IdentityCli, IdentityCli>();
            return services;
        }
    }
}
