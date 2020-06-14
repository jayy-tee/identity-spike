namespace Identity.TestSdk.ResponseModels
{
    public class UserProfileResponse
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string username { get; set; }
        public string emailAddress { get; set; }
        public int userStatus { get; set; }
        public int source { get; set; }
    }
}