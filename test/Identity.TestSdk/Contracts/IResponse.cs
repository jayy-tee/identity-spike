using System.Net;

namespace Identity.TestSdk.Contracts
{
    public interface IResponse
    {
        HttpStatusCode StatusCode { get; }
        string Content { get; }
        T As<T>() where T : class;
    }
}
