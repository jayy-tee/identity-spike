using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.Application.Users;
using Identity.Common;
using Identity.Domain.UserAggregate;
using Identity.TestSdk.TestData;


namespace Identity.TestSdk.Mocks
{
    public class MockUserRepository : IUserRepository
    {
        private List<User> _users;
        public UserSource SourceSystem { get; private set; }

        public MockUserRepository()
        {
            _users = new List<User>() {
                ValidUser.AsUser(),
                InvalidUser.AsUser()
            };
            SourceSystem = UserSource.New;
        }

        public async Task<User> Get(string username) =>
            await Task.FromResult(_users.Where(u => u.Username == username).FirstOrDefault());

        public async Task<bool> CheckExists(string username) =>
            await Task.FromResult(_users.Where(u => u.Username == username).Count() > 0);

        public async Task<bool> AddUser(User user) {
            _users.Add(user);
            return await Task.FromResult(true);
        }

    }
}