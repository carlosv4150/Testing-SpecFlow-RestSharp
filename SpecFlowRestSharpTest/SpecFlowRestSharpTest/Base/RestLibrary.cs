using GraphQLProductApp;
using Microsoft.AspNetCore.Mvc.Testing;
using RestSharp;

namespace SpecFlowRestSharpTest.Base;

public interface IRestLibrary
{
    RestClient RestClient { get; }
}

public class RestLibrary : IRestLibrary
{
    public RestLibrary(WebApplicationFactory<GraphQLProductApp.Startup> webApplicationFactory)
    {
        var restClientOpt = new RestClientOptions
        {
            BaseUrl = new Uri("https://localhost:5001"),
            RemoteCertificateValidationCallback = (sender, certificate, chain, error) => true
        };

        //Spawn SUT
        var client = webApplicationFactory.CreateDefaultClient(); 

        // Rest Client
        RestClient = new RestClient(client, restClientOpt);
    }

    public RestClient RestClient { get; }

}
