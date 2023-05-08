using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using SpecFlowRestSharpTest.Base;

namespace SpecFlowRestSharpTest;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddSingleton<IRestLibrary>(new RestLibrary(new WebApplicationFactory<GraphQLProductApp.Startup>()))
            .AddScoped<IRestBuilder, RestBuilder>()
            .AddScoped<IRestFactory, RestFactory>();
    }
}
