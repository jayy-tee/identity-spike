using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using Identity.Api.Dto;
using Identity.Domain.UserAggregate;
using Identity.Common;

namespace Identity.Api.Core.Facades
{
    public class UserFacade : IUserFacade
    {
        private readonly IEnumerable<IUserRepository> _repos;
        private readonly ILogger<IUserFacade> _logger;

        public UserFacade(ILogger<IUserFacade> logger, IEnumerable<IUserRepository> repos)
        {
            _repos = repos;
            _logger = logger;
        }

        public async Task<User> GetUser(string username)
        {
            var userSource = await FindUserSource(username);
            if (userSource == UserSource.None)
            {
                return null;
            }

            return await _repos.Where(x => x.SourceSystem == userSource).First().Get(username);
        }

        public async Task<User> ValidateCredentials(UserCredential credential)
        {
            var userSource = await FindUserSource(credential.Username);
            if (userSource == UserSource.None)
            {
                return null;
            }

            var user = await _repos.Where(x => x.SourceSystem == userSource).First().Get(credential.Username);
            if (user.ValidatePassword(credential.Password))
                return user;

            return null;
        }

        public async Task<User> NewUser(NewUserDto request){
            var user = new User(
                request.FirstName, request.LastName, request.Username, 
                request.EmailAddress, UserStatus.Enabled, request.Password, (UserSource)request.System);
            
            var result = await _repos.Where(x => x.SourceSystem == user.Source).First().AddUser(user);
            if (!result){
                return null;
            }

            return user;
        }

        private async Task<UserSource> FindUserSource(string username)
        {
            // introduce in-mem caching here to avoid extraneous database calls
            var sourceCount = new Dictionary<UserSource, bool>();
            foreach (IUserRepository repo in _repos)
            {
                sourceCount.Add(repo.SourceSystem, await repo.CheckExists(username));
            }

            return sourceCount.Where(x => x.Value == true).FirstOrDefault().Key;
        }

    }
}