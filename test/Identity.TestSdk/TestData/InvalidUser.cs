using Identity.Common;
using Identity.Domain.UserAggregate;

namespace Identity.TestSdk.TestData
{
    public static class InvalidUser
    {
        public static string Firstname = "theFirstName";
        public static string Lastname = "theLastName";
        public static string Username = "theInvalidUser";
        public static string EmailAddress = "theInvalidUser@somedomain.com";
        public static string Password = "thePassword";
        public static UserStatus Status = UserStatus.Disabled;
        public static UserSource UserSource = UserSource.New;

        public static User AsUser() => new User(
                   Firstname, Lastname, Username,
                   EmailAddress, Status, Password,
                   UserSource);
    }
}