using Identity.Common;

namespace Identity.Domain.UserAggregate
{
    public class User
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Username { get; private set; }
        public string EmailAddress { get; private set; }
        public UserStatus UserStatus { get; private set; }
        public UserSource Source { get; private set; }

        private string _passwordHash;
        private string _passwordSalt;

        public User(string firstname, string lastname, string username,
                    string emailAddress, UserStatus userStatus,
                    string password, UserSource source)
        {
            FirstName = firstname;
            LastName = lastname;
            Username = username;
            EmailAddress = emailAddress;
            UserStatus = userStatus;
            Source = source;

            SetPassword(password);
        }

        private User() { }

        public bool ValidatePassword(string password)
        {
            if (_passwordHash == GenerateHash(password))
                return true;
            return false;
        }

        public void Enable()
        {
            UserStatus = UserStatus.Enabled;
        }

        public void Disable()
        {
            UserStatus = UserStatus.Disabled;
        }

        public void SetPassword(string password)
        {
            if (Source == UserSource.New)
            {
                _passwordSalt = PasswordHelper.GenerateSalt();
            }
            _passwordHash = GenerateHash(password);
        }

        public string GetPasswordHash()
        {
            return _passwordHash;
        }

        public string GetPasswordSalt()
        {
            return _passwordSalt;
        }

        private string GenerateHash(string password)
        {
            switch (Source)
            {
                case UserSource.Legacy:
                    return PasswordHelper.EncodeLegacyPassword(password);
                case UserSource.New:
                    return PasswordHelper.EncodeMembershipPassword(password, _passwordSalt);
                default:
                    throw new System.Exception("Could not find appropiate hashing algorithm.");
            }
        }
    }
}