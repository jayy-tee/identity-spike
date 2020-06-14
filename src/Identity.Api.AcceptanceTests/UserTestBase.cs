using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Identity.TestSdk.RequestBuilders;
using Identity.TestSdk.ResponseModels;

namespace Identity.Api.AcceptanceTests
{
    [TestClass]
    public abstract class UserTestBase : AcceptanceTestBase
    {
        public string testUserUsername;
        public string testUserPassword;
        public string testUserUrl;

        public virtual void SetupTest()
        {
            testUserUsername = $"theUserName";
            testUserPassword = $"thePassword";
            testUserUrl = $"/api/user/{testUserUsername}";
        }

        [TestMethod]
        [TestCategory("Contract")]
        public void WhenUserExists_ProfileIsReturned()
        {
            // Arrange
            var request = new UserRequestBuilder()
                .GetUser(testUserUsername)
                .Build();

            // Act
            var response = Client.Execute(request, andExpect: System.Net.HttpStatusCode.OK).As<UserProfileResponse>();

            // Assert           
            response.Username.Should().Be(testUserUsername);
            response.FirstName.Should().NotBeNullOrEmpty().And.BeOfType(typeof(string));
            response.LastName.Should().NotBeNullOrEmpty().And.BeOfType(typeof(string));
            response.EmailAddress.Should().NotBeNullOrEmpty().And.BeOfType(typeof(string));
            response.Source.Should().BeOfType(typeof(int)).And.BeOneOf(1,2);
            response.UserStatus.Should().BeOfType(typeof(int)).And.BeOneOf(0,1);
        }

        [TestMethod]
        [TestCategory("Contract")]
        public void WhenUserDoesntExist_WeGetNotFound()
        {
            // Arrange
            const string NonExistentUser = "fakeuserdontexist";
            var request = new UserRequestBuilder()
                .GetUser(NonExistentUser)
                .Build();

            // Act
            Client.Execute(request, andExpect: System.Net.HttpStatusCode.NotFound);
        }

        [TestMethod]
        public void WhenWeAuthenticateWithValidCredentials_WeGetAnOkResponse()
        {
            // Arrange
            var request = new UserRequestBuilder()
                .Authenticate(testUserUsername)
                .WithPassword(testUserPassword)
                .Build();

            // Act
            Client.Execute(request, andExpect: System.Net.HttpStatusCode.OK);
        }

        [TestMethod]
        public void WhenWeAuthenticateWithInValidCredentials_WeAreDenied()
        {
            // Arrange
            var request = new UserRequestBuilder()
                .Authenticate(testUserUsername)
                .WithPassword(testUserUsername)
                .Build();

            // Act
            Client.Execute(request, andExpect: System.Net.HttpStatusCode.Unauthorized);
        }
    }
}
