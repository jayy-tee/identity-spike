using Identity.TestSdk.Contracts;

namespace Identity.TestSdk.Models
{
    public class Request : IRequest
    {
        public Request()
        {
        }

        public string Body { get; set; }
        public RestSharp.Method Method { get; set; }
        public string RelativeUrl { get; set; }

    }
}
