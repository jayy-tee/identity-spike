namespace Identity.TestSdk.ResponseModels
{
    public class UserProfileResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public int UserStatus { get; set; }
        public int Source { get; set; }
    }
}