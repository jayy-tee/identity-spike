using System.Threading.Tasks;
using Identity.Common;
using Identity.Domain.UserAggregate;

namespace Identity.Application.Users
{
    public interface IUserRepository
    {
        Task<User> Get(string username);
        UserSource SourceSystem {get; }
        Task<bool> CheckExists(string username);
        Task<bool> AddUser(User user);
    }
}