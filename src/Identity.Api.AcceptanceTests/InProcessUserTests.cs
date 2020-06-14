using Identity.Application.Users;
using Identity.TestSdk.Mocks;
using Identity.TestSdk.RequestBuilders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;


namespace Identity.Api.AcceptanceTests
{
    [TestClass]
    [TestCategory("RequiresInProcess")]
    public class UserTests : UserTestBase
    {
        protected override bool IsInProcessOnly => true;
        protected override void ConfigureServices(IServiceCollection services)
        {
            var existingDescriptors = services.Where(s => s.ServiceType == typeof(IUserRepository));
            existingDescriptors.ToList().ForEach(descriptor =>
            {
                services.Remove(descriptor);
            });

            services.AddSingleton<IUserRepository, MockUserRepository>();
            services.AddSingleton<IUserRepository, MockLegacyRepository>();
        }

        public override void SetupTest()
        {
            var dateString = DateTime.Now.ToString("yyyyMMddHHmmss");
            testUserUsername = $"acceptancetestuser{dateString}";
            testUserPassword = $"thePassword{dateString}";
            testUserUrl = $"/api/user/{testUserUsername}";
        }

        [TestInitialize]
        public void WhenRunningTests_CreateAUser()
        {
            // Arrange
            SetupTest();
            var request = new UserRequestBuilder()
                    .CreateUser()
                    .UsingTestData()
                    .WithUsername(testUserUsername)
                    .WithPassword(testUserPassword)
                    .Build();

            // Act
            Client.Execute(request, andExpect: System.Net.HttpStatusCode.Created);
        }

        [TestMethod]
        public void WhenAddingAUserAndItExists_WeGetBadRequest()
        {
            // Arrange
            var request = new UserRequestBuilder()
                    .CreateUser()
                    .UsingTestData()
                    .WithUsername(testUserUsername)
                    .WithPassword(testUserPassword)
                    .Build();

            // Act/Assert
            Client.Execute(request, andExpect: System.Net.HttpStatusCode.BadRequest);
        }

    }
}
