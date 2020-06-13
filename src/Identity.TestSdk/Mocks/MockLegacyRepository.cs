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
    public class MockLegacyRepository : IUserRepository
    {
        private List<User> _users;
        public UserSource SourceSystem { get; private set; }

        public MockLegacyRepository()
        {
            _users = new List<User>() {
                ValidLegacyUser.AsUser(),
                InvalidLegacyUser.AsUser()
            };
            SourceSystem = UserSource.Legacy;
        }

        public async Task<User> Get(string username) =>
             _users.Where(u => u.Username == username).FirstOrDefault();
             
        public async Task<bool> CheckExists(string username) =>
            _users.Where(u => u.Username == username).Count() > 0;
        
        public async Task<bool> AddUser(User user) =>
            throw new NotImplementedException();

    }
}