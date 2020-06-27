using Identity.Application.Users;
using Identity.TestSdk.Mocks;
using Identity.TestSdk.RequestBuilders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Net;


namespace Identity.Api.AcceptanceTests
{
    [TestClass]
    [TestCategory("RequiresInProcess")]
    public class InProcessUserTests : UserTestBase
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

        [TestInitialize]
        public void WhenRunningTests_CreateAUserIfItDoesntExist()
        {
            // Setup and check if test user exists
            SetupTest();
            var request = new UserRequestBuilder()
                    .GetUser(testUserUsername)
                    .Build();

            var response = Client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
                return;

            // Create test user
            var newUserRequest = new UserRequestBuilder()
                    .CreateUser()
                    .UsingTestData()
                    .WithUsername(testUserUsername)
                    .WithPassword(testUserPassword)
                    .Build();
            Client.Execute(newUserRequest, andExpect: System.Net.HttpStatusCode.Created);
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

        [TestMethod]
        public void WhenAddingAUserWithInvalidData_WeGetBadRequest()
        {
            // Arrange
            var request = new UserRequestBuilder()
                    .CreateUser()
                    .Build();

            // Act/Assert
            Client.Execute(request, andExpect: System.Net.HttpStatusCode.BadRequest);
        }

    }
}
