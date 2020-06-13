using Identity.Common;
using Identity.Domain.UserAggregate;

namespace Identity.Domain.UnitTests.TestData
{
    public static class ValidUser
    {
        public static string Firstname = "theFirstName";
        public static string Lastname = "theLastName";
        public static string Username = "theUser";
        public static string EmailAddress = "theUser@somedomain.com";
        public static string Password = "thePassword";
        public static UserStatus Status = UserStatus.Enabled;
        public static UserSource UserSource = UserSource.New;

        public static User AsUser() => new User(
                   Firstname, Lastname, Username,
                   EmailAddress, Status, Password,
                   UserSource);
    }
}