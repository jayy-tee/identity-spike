using System.Threading.Tasks;
using Identity.Common;

namespace Identity.Domain.UserAggregate
{
    public interface IUserRepository
    {
        Task<User> Get(string username);
        UserSource SourceSystem {get; }
        Task<bool> CheckExists(string username);
        Task<bool> AddUser(User user);
    }
}