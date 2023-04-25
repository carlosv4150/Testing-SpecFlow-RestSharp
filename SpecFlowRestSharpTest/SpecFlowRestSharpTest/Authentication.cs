using FluentAssertions;
using GraphQLProductApp.Data;
using Newtonsoft.Json.Linq;
using RestSharp;
using Xunit;

namespace SpecFlowRestSharpTest;

public class Authentication

{
    private RestClientOptions _restClientOpt;

    public Authentication()
    {
        _restClientOpt = new RestClientOptions
        {
            BaseUrl = new Uri("https://localhost:5001"),
            RemoteCertificateValidationCallback = (sender, certificate, chain, error) => true
        };
    }

    [Fact]
    public async Task getWithQueryParameterTest()
    {
        // Rest Client
        var client = new RestClient(_restClientOpt);

        // Rest Request
        var authRequest = new RestRequest("/api/Authenticate/Login");

        // Anonymous object being passed as body in request
        authRequest.AddJsonBody(new
        {
            username = "carlos",
            password = "1234"
        });

        /* A different alternative: Typed object being passed as body in request
         * request.AddJsonBody(new LoginModel
        {
            UserName = "carlos",
            Password = "1234"
        });
        */

        //Perform GET
        var authResponse = await client.PostAsync(authRequest);
        var token = JObject.Parse(authResponse.Content)["token"];

        // Rest Request
        var productGetRequest = new RestRequest("/Product/GetProductById/1");
        productGetRequest.AddHeader("Accept", "application/json");
        productGetRequest.AddHeader("Authorization", $"Bearer {token.ToString()}");

        //Perform GET
        var productResponse = await client.GetAsync<Product>(productGetRequest);

        //Assert
        productResponse?.Name.Should().Be("Keyboard");
    }
}
