using FluentAssertions;
using GraphQLProductApp.Data;
using Newtonsoft.Json.Linq;
using SpecFlowRestSharpTest.Base;
using Xunit;

namespace SpecFlowRestSharpTest;
public class BasicTests

{
    private readonly IRestFactory _restFactory;
    private readonly string? _token;
    public BasicTests(IRestFactory restFactory)
    {
        _restFactory = restFactory;
        _token = GetToken();
    }

    [Fact]
    public async Task GetOperationTest()
    {
        var response = await _restFactory.Create()
            .WithRequest("/Product/GetProductById/1")
            .WithHeader("Authorization", $"Bearer {_token}")
            .WithGet<Product>();

        //Assert
        response?.Name.Should().Be("Keyboard");
    }

    [Fact]
    public async Task GetWithQuerySegmentTest()
    {
        var response = await _restFactory.Create()
            .WithRequest("/Product/GetProductById/{id}")
            .WithHeader("Authorization", $"Bearer {_token}")
            .WithUrlSegment("id", "2")
            .WithGet<Product>();

        //Assert
        response?.Price.Should().Be(400);
    }

    [Fact]
    public async Task GetWithQueryParameterTest()
    {
        var response = await _restFactory.Create()
            .WithRequest("/Product/GetProductByIdAndName/{id}")
            .WithHeader("Authorization", $"Bearer {_token}")
            .WithQueryParameter("id", "2")
            .WithQueryParameter("name", "Monitor")
            .WithGet<Product>();

        //Assert
        response?.Price.Should().Be(400);
    }

    [Fact]
    public async Task PostTest()
    {
        var response = await _restFactory.Create()
            .WithRequest("Product/Create")
            .WithHeader("Authorization", $"Bearer {_token}")
            .WithBody(new Product
            {
                Name = "Printer",
                Description = "Color Printer",
                Price = 500,
                ProductType = ProductType.PERIPHARALS
            })
            .WithPost<Product>();

        //Assert
        response?.Price.Should().Be(500);
    }

    /*[Fact]
    public async Task FileUploadTest()
    {
        var response = await _restFactory.Create()
            .WithRequest("Product")
            .WithHeader("Authorization", $"Bearer {_token}")
            .WithFile("myFile", @"C:\git\Dotnet\TestImage\restsharp.png", "multipart/form-data")
            .WithPost();// pending to check if is necessary a diferent method to upload file
        // Rest Request
        /*var request = new RestRequest("Product", Method.Post);
        request.AddHeader("Authorization", $"Bearer {GetToken()}");
        request.AddFile("myFile", @"C:\git\Dotnet\TestImage\restsharp.png", "multipart/form-data");

        //Perform POST
        var response = await _client.ExecuteAsync(request);

        //Assert
        response?.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
    }*/

    private string GetToken()
    {
        var authResponse = _restFactory
            .Create()
            .WithRequest("api/Authenticate/Login")
            .WithBody(new
            {
                username = "carlos",
                password = "1234"
            })
            .WithPost().Result.Content;

        return JObject.Parse(authResponse)["token"].ToString();
    }
}
