using System.Threading.Tasks;
using Identity.Common;
using Identity.Application.Users.Model;

namespace Identity.Application.Users
{
    public interface IUserFacade {
        Task<UserDto> GetUser(string username);
        Task<UserDto> ValidateCredentials(UserCredential credential);
        Task<UserDto> NewUser(NewUserDto request);
    }
}