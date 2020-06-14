using Serilog;
using System.Net;
using Identity.TestSdk.Contracts;
using Identity.TestSdk.Infrastructure;
using Identity.TestSdk.Models;

namespace Identity.TestSdk.Services
{
    public class Client : IClient
    {
        private readonly TestSettings _testSettings;
        private readonly ILogger _logger;

        public Client(TestSettings testSettings, ILogger logger)
        {
            _testSettings = testSettings;
            _logger = logger;
        }

        public IResponse Execute(IRequest request, HttpStatusCode andExpect)
        {
            var rc = new RestSharp.RestClient(_testSettings.BaseUrl);
            var rr = new RestSharp.RestRequest(request.RelativeUrl, request.Method);

            if(request.Body != null)
            {
                rr.AddJsonBody(request.Body);
            }

            _logger.Information("{Event} {HttpMethod} {BaseUrl}{RelativeUrl} {@request}", "TestStartHttpRequest", request.Method, _testSettings.BaseUrl, request.RelativeUrl, request);
            var response = rc.Execute(rr);

            _logger.Write(response.StatusCode == andExpect ? 
                Serilog.Events.LogEventLevel.Information : 
                Serilog.Events.LogEventLevel.Error,
                "{Event} {HttpMethod} {BaseUrl}{RelativeUrl} {StatusCode} {ExpectedStatusCode}", "TestEndHttpRequest", request.Method, _testSettings.BaseUrl, request.RelativeUrl, response.StatusCode, andExpect);

            if (response.StatusCode != andExpect) throw new WebException($"The call to {request.Method} {_testSettings.BaseUrl}{request.RelativeUrl} was expected to return {andExpect} but returned {response.StatusCode}");

            return new Response(response);
        }
    }
}
