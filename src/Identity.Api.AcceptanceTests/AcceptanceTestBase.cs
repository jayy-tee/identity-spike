using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Identity.Api;
using Identity.Api.AcceptanceTests.Infrastructure;
using Identity.TestSdk.Contracts;
using Identity.TestSdk.Infrastructure;

namespace Identity.Api.AcceptanceTests
{
    /// <summary>
    /// This implementation uses a 'hard' port binding, a real HTTP Servier and HTTP Client. 
    /// </summary>
    [TestClass]
    public class AcceptanceTestBase
    {
        public TestContext TestContext { get; set; }

        protected IHost Host => _host ?? throw new InvalidOperationException($"You must initialize the host before accessing it. ");

        protected IClient Client => Resolve<IClient>();

        private IHost _host;

        private IServiceScope _scope;

        [TestInitialize]
        public async Task SetupAcceptanceTestBase()
        {

            // Create a new Lifetime Scope for the test: anything in the container with .AddScoped() will be resolved exactly once within each Scope
            _scope = ContainerSingleton.Instance().CreateScope();

            var testSettings = Resolve<TestSettings>();

            if (IsInProcessOnly && !testSettings.InProcess)
            {
                Assert.Inconclusive($"The test called {TestContext.FullyQualifiedTestClassName} can only be run InProcess; the current TestExecutionContext is not InProcess. Therefore, the test is being skipped. To avoid this message, consider using a TestCategory on the test and explicitly ignoring it in the Test Filter. ");
            }

            if (SkipIfInProcess && testSettings.InProcess)
            {
                Assert.Inconclusive($"The test called {TestContext.FullyQualifiedTestClassName} can only be run when not InProcess; the current TestExecutionContext is InProcess. Therefore, the test is being skipped.");
            }

            foreach (var key in testSettings.EnvironmentVariables.Keys)
            {
                Environment.SetEnvironmentVariable(key, testSettings.EnvironmentVariables[key], EnvironmentVariableTarget.Process);
            }

            Environment.SetEnvironmentVariable("ASPNETCORE_URLS", testSettings.BaseUrl, EnvironmentVariableTarget.Process);

            if (testSettings.InProcess)
            {
                _host = Program
                    .CreateHostBuilder()
                    .ConfigureServices(services =>
                    {
                        ConfigureServices(services);
                    })
                    .Build();

                await _host.StartAsync();
            }
        }

        [TestCleanup]
        public async Task CleanupAcceptanceTestBase()
        {
            try
            {
                if (_host != null)
                {
                    await _host.StopAsync();
                    _host.Dispose();
                    _host = null;
                }
            }
            finally
            {
                _scope.Dispose();
            }
        }

        protected T Resolve<T>()
        {
            return (T)_scope.ServiceProvider.GetRequiredService(typeof(T));
        }

        protected virtual void ConfigureServices(IServiceCollection services)
        {
        }

        protected virtual bool IsInProcessOnly => false;
        protected virtual bool SkipIfInProcess => false;
    }
}
