using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Identity.Application;
using Identity.Application.Users;
using Identity.Cli.Config;
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
            var configuration = ConfigurationSingleton.Instance;
            services.AddLogging(configure => configure.AddConsole());
            services.AddApplication();
            services.AddInfrastructure(configuration);
            services.AddTransient<IdentityCli, IdentityCli>();
            return services;
        }
    }
}
