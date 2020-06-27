using Identity.Common;

namespace Identity.Application.Users.Model
{
    public class UserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public UserStatus UserStatus { get; set; }
        public UserSource Source { get; set; }
    }
}
