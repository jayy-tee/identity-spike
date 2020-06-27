using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using Serilog.Extensions.Logging;
using System;
using System.IO;
using Identity.TestSdk.Infrastructure;

namespace Identity.Api.AcceptanceTests.Infrastructure
{
    /// <summary>
    /// </summary>
    public static class ContainerSingleton
    {
        public static IServiceProvider _instance;

        public static IServiceProvider Instance()
        {
            return _instance ?? throw new System.InvalidOperationException($"You must call InitializeContainer first. ");
        }

        public static IServiceProvider InitializeContainer(string testExecutionContextFilename)
        {
            if (_instance != null) throw new System.InvalidOperationException($"You can only call InitializeContainer once. ");

            var services = new ServiceCollection();
            var configuration = BuildTestExecutionContext(testExecutionContextFilename);
            var testSettings = configuration.GetSection("TestSettings").Get<TestSettings>();

            services.AddSingleton(testSettings);
            services.AddScoped(sp => 
            {
                var serilogContext = BuildSerilogConfiguration();

                Serilog.ILogger logger = new LoggerConfiguration()
                    .ReadFrom
                    .Configuration(serilogContext)
                    .Enrich
                    .FromLogContext()
                    .CreateLogger();

                return logger; 
            });

            Identity.Api.AcceptanceTests.Infrastructure.Container.Populate(services, testSettings);

            var result = services.BuildServiceProvider();
            _instance = result;

            return result;
        }

        private static IConfigurationRoot BuildTestExecutionContext(string testExecutionContextFilename)
        {
            var testExecutionContextRelativePath = Path.Combine("TestSettings", $"{testExecutionContextFilename}");
            var testExecutionContextFullPath = Path.Combine(Directory.GetCurrentDirectory(), testExecutionContextRelativePath);
            if (!File.Exists(testExecutionContextFullPath))
            {
                Assert.Fail($"The TestExecutionContext file does not exist at '{testExecutionContextFullPath}");
            }

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(Path.Combine("TestSettings", "testSettings.json"), optional: false)
                .AddJsonFile(testExecutionContextRelativePath, optional: true)
                .Build();

            return configuration;
        }

        private static IConfigurationRoot BuildSerilogConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("serilogSettings.json", optional: false)
                .Build();

            return configuration;
        }
    }
}
