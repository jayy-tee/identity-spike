using Microsoft.Extensions.DependencyInjection;
using Identity.TestSdk.Contracts;
using Identity.TestSdk.Infrastructure;
using Identity.TestSdk.Services;

namespace Identity.Api.AcceptanceTests.Infrastructure
{
    public static class Container
    {
        public static void Populate(IServiceCollection container, TestSettings testSettings)
        {
            container.AddScoped<IClient, Client>();
        }
    }
}
