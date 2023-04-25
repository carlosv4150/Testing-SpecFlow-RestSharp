using FluentAssertions;
using GraphQLProductApp.Data;
using Newtonsoft.Json.Linq;
using RestSharp;
using Xunit;
using Xunit.Abstractions;

public class BasicTests

{
    private RestClientOptions _restClientOpt;

    public BasicTests(ITestOutputHelper output)
    {
        _restClientOpt = new RestClientOptions
        {
            BaseUrl = new Uri("https://localhost:5001"),
            RemoteCertificateValidationCallback = (sender, certificate, chain, error) => true
        };
    }

    [Fact]
    public async Task GetOperationTest()
    {

        // Rest Client
        var client = new RestClient(_restClientOpt);

        // Rest Request
        var request = new RestRequest("/Product/GetProductById/1");
        request.AddHeader("Authorization", $"Bearer {GetToken()}");

        //Perform GET
        var response = await client.GetAsync<Product> (request);

        //Assert
        response?.Name.Should().Be("Keyboard");
    }

    [Fact]
    public async Task GetWithQuerySegmentTest()
    {

        // Rest Client
        var client = new RestClient(_restClientOpt);

        // Rest Request
        var request = new RestRequest("/Product/GetProductById/{id}");
        request.AddHeader("Authorization", $"Bearer {GetToken()}");
        request.AddUrlSegment("id", 2);

        //Perform GET
        var response = await client.GetAsync<Product>(request);

        //Assert
        response?.Price.Should().Be(400);
    }

    [Fact]
    public async Task GetWithQueryParameterTest()
    {

        // Rest Client
        var client = new RestClient(_restClientOpt);

        // Rest Request
        var request = new RestRequest("/Product/GetProductByIdAndName/{id}");
        request.AddHeader("Authorization", $"Bearer {GetToken()}");
        request.AddQueryParameter("id", 2);
        request.AddQueryParameter("name", "Monitor");

        //Perform GET
        var response = await client.GetAsync<Product>(request);

        //Assert
        response?.Price.Should().Be(400);
    }

    [Fact]
    public async Task PostTest()
    {
        // Rest Client
        var client = new RestClient(_restClientOpt);

        // Rest Request
        var request = new RestRequest("/Product/Create");
        request.AddHeader("Authorization", $"Bearer {GetToken()}");
        request.AddJsonBody(new Product
        {
            Name = "Printer",
            Description = "Color Printer",
            Price = 500,
            ProductType = ProductType.PERIPHARALS
        });

        //Perform POST
        var response = await client.PostAsync<Product>(request);

        //Assert
        response?.Price.Should().Be(500);
    }

    private string GetToken()
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

        //Perform POST
        var authResponse = client.PostAsync(authRequest).Result.Content;

        return JObject.Parse(authResponse)["token"].ToString();
    }
}
