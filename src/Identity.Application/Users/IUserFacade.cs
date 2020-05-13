using System.Threading.Tasks;
using Identity.Domain.UserAggregate;
using Identity.Common;
using Identity.Application.Users.Model;

namespace Identity.Application.Users
{
    public interface IUserFacade {
        Task<User> GetUser(string username);
        Task<User> ValidateCredentials(UserCredential credential);
        Task<User> NewUser(NewUserDto request);
    }
}