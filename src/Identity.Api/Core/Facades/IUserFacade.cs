using System.Threading.Tasks;
using Identity.Domain.UserAggregate;
using Identity.Common;
using Identity.Api.Dto;

namespace Identity.Api.Core.Facades
{
    public interface IUserFacade {
        Task<User> GetUser(string username);
        Task<User> ValidateCredentials(UserCredential credential);
        Task<User> NewUser(NewUserDto request);
    }
}