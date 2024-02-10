using Identity.TestSdk.Contracts;
using Identity.TestSdk.Models;
using Identity.TestSdk.RequestModels;
using Identity.TestSdk.TestData;
using System.Text.Json;
using RestSharp;

namespace Identity.TestSdk.RequestBuilders
{
    public class UserRequestBuilder
    {
        private Method _method;
        private string _relativeUrl;
        private User _user;

        public UserRequestBuilder()
        {
            _user = new User();
            _method = Method.Get;
            _relativeUrl = "/fake/url";
        }

        public UserRequestBuilder User()
        {
            _user = new User();
            return this;
        }

        public UserRequestBuilder CreateUser()
        {
            _method = Method.Post;
            _relativeUrl = "/api/user/new";
            
            return this;
        }

        public UserRequestBuilder GetUser(string username)
        {
            _method = Method.Get;
            _relativeUrl = $"/api/user/{username}";
            _user = null;

            return this;
        }

        public UserRequestBuilder Authenticate(string username)
        {
            _method = Method.Post;
            _user.Username = username;
            _relativeUrl = $"/api/user/{_user.Username}/authenticate";

            return this;
        }

        public UserRequestBuilder UsingTestData()
        {
            _user.EmailAddress = ValidUser.EmailAddress;
            _user.FirstName = ValidUser.Firstname;
            _user.LastName = ValidUser.Lastname;
            _user.Username = ValidUser.Username;
            _user.Password = ValidUser.Password;

            return this;
        }

        public UserRequestBuilder WithUsername(string username)
        {
            _user.Username = username;

            return this;
        }

        public UserRequestBuilder WithPassword(string password)
        {
            _user.Password = password;

            return this;
        }

        public IRequest Build()
        {
            return new Request()
            {
                Method = _method,
                RelativeUrl = _relativeUrl,
                Body = _user != null ? JsonSerializer.Serialize(_user) : null
            };
        }
    }
}