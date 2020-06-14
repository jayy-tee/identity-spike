using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Identity.TestSdk.RequestBuilders;
using Identity.TestSdk.ResponseModels;
using System.Text.Json;
using System.Net;

namespace Identity.Api.AcceptanceTests
{
    [TestClass]
    [TestCategory("SkipIfInProcess")]
    public class UserTests : UserTestBase
    {

        [TestInitialize]
        public void WhenRunningTests_CreateAUserIfItDoesntExist()
        {
            SkipIfOutOfProcessOnly();
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

    }
}
