namespace Identity.TestSdk.Contracts
{
    public interface IRequest
    {
        RestSharp.Method Method { get; set; }
        string RelativeUrl { get; set; }
        string Body { get; set; }
    }
}
