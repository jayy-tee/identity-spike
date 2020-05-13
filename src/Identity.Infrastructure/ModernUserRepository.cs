using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.Application.Users;
using Identity.Common;
using Identity.Domain.UserAggregate;


namespace Identity.Infrastructure
{
    public class ModernUserRepository : IUserRepository
    {
        private List<User> _users;
        public UserSource SourceSystem { get; private set; }

        public ModernUserRepository()
        {
            _users = new List<User>() {
                 new User("Joe", "Bloggs", "jbloggs1", "fake@fake.com", UserStatus.Disabled, "blah", UserSource.New),
                 new User("Joe", "Bloggs", "jbloggs", "fake@fake.com", UserStatus.Enabled, "blah", UserSource.New)
            };
            SourceSystem = UserSource.New;
        }

        public async Task<User> Get(string username) =>
             _users.Where(u => u.Username == username).FirstOrDefault();
             
        public async Task<bool> CheckExists(string username) =>
            _users.Where(u => u.Username == username).Count() > 0;
        
        public async Task<bool> AddUser(User user) =>
            throw new NotImplementedException();

    }
}