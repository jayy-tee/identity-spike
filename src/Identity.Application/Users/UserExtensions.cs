using Identity.Application.Users.Model;
using Identity.Domain.UserAggregate;

namespace Identity.Application.Users 
{
    public static class UserExtensions
    {
        public static UserDto MapToDto (this User user)
        {
            return new UserDto{
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                EmailAddress = user.EmailAddress,
                UserStatus = user.UserStatus,
                Source = user.Source
            };
        }
    }
}