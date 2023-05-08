using System.Xml.Linq;
using RestSharp;

namespace SpecFlowRestSharpTest.Base;

public interface IRestBuilder
{
    IRestBuilder WithBody(object body);
    Task<T> WithDelete<T>();
    Task<T> WithGet<T>();
    IRestBuilder WithHeader(string name, string value);
    Task<T> WithPatch<T>();
    Task<T> WithPost<T>();
    Task<RestResponse> WithPost();
    Task<RestResponse> WithUpload();
    Task<T> WithPut<T>();
    IRestBuilder WithQueryParameter(string name, string value);
    IRestBuilder WithRequest(string request);
    IRestBuilder WithUrlSegment(string name, string value);
    IRestBuilder WithFile(string name, string path, string format);

}

public class RestBuilder : IRestBuilder
{
    private readonly IRestLibrary _restLibrary;
    public RestBuilder(IRestLibrary restLibrary)
    {
        _restLibrary = restLibrary;
    }

    private RestRequest RestRequest { get; set; } = null!;

    public IRestBuilder WithRequest(string request)
    {
        RestRequest = new RestRequest(request);
        return this;
    }

    public IRestBuilder WithHeader(string name, string value)
    {
        RestRequest.AddHeader(name, value);
        return this;
    }

    public IRestBuilder WithQueryParameter(string name, string value)
    {
        RestRequest.AddQueryParameter(name, value);
        return this;
    }
    public IRestBuilder WithUrlSegment(string name, string value)
    {
        RestRequest.AddUrlSegment(name, value);
        return this;
    }

    public IRestBuilder WithBody(object body)
    {
        RestRequest.AddJsonBody(body);
        return this;
    }
    public async Task<T?> WithGet<T>()
    {
        return await _restLibrary.RestClient.GetAsync<T>(RestRequest);
    }
    public async Task<T?> WithPost<T>()
    {
        return await _restLibrary.RestClient.PostAsync<T>(RestRequest);
    }
    public async Task<RestResponse> WithPost()
    {
        return await _restLibrary.RestClient.PostAsync(RestRequest);
    }
    public async Task<T?> WithPut<T>()
    {
        return await _restLibrary.RestClient.PutAsync<T>(RestRequest);
    }
    public async Task<T?> WithDelete<T>()
    {
        return await _restLibrary.RestClient.DeleteAsync<T>(RestRequest);
    }
    public async Task<T?> WithPatch<T>()
    {
        return await _restLibrary.RestClient.PatchAsync<T>(RestRequest);
    }
    public IRestBuilder WithFile(string name, string path, string format)
    {
        RestRequest.AddFile(name, path, format);
        return this;
    }
    public async Task<RestResponse> WithUpload()
    {
        return await _restLibrary.RestClient.ExecuteAsync(RestRequest);
    }
}
