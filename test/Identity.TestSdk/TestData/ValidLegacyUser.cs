using Identity.Common;
using Identity.Domain.UserAggregate;

namespace Identity.TestSdk.TestData
{
    public static class ValidLegacyUser
    {
        public static string Firstname = "theFirstName";
        public static string Lastname = "theLastName";
        public static string Username = "theLegacyUser";
        public static string EmailAddress = "theLegacyUser@somedomain.com";
        public static string Password = "thePassword";
        public static UserStatus Status = UserStatus.Enabled;
        public static UserSource UserSource = UserSource.Legacy;

        public static User AsUser() => new User(
                   Firstname, Lastname, Username,
                   EmailAddress, Status, Password,
                   UserSource);
    }
}