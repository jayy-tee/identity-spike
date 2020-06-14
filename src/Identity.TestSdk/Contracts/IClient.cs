using System.Net;

namespace Identity.TestSdk.Contracts
{
    public interface IClient
    {
        IResponse Execute(IRequest request, HttpStatusCode andExpect);
    }
}
