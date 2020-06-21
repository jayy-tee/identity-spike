using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Identity.Infrastructure.Config
{
    /// <summary>
    /// </summary>
    public class ConfigurationSingleton
    {
        public const string ENVIRONMENT_VARIABLE_PREFIX = "IDSPIKE";

        private static IConfiguration _instance;
        private static readonly object _padlock = new object();

        public static IConfiguration Instance
        {
            get
            {
                lock (_padlock)
                {
                    if (null != _instance)
                    {
                        return _instance;
                    }

                    _instance = BuildConfiguration();

                    return _instance;
                }
            }
        }

        protected static IConfiguration BuildConfiguration()
        {
            var result = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Local"}.json", optional: true)
                .AddJsonFile("serilogSettings.json", optional: false, reloadOnChange: false)
                .AddEnvironmentVariables(ENVIRONMENT_VARIABLE_PREFIX)
                .Build();

            return result;
        }
    }
}
