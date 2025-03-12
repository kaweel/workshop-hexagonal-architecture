
using Microsoft.AspNetCore.Mvc.Testing;


public abstract class TestControllerBase : IClassFixture<WebApplicationFactory<Program>>
{
    protected readonly HttpClient _client;

    public TestControllerBase(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

}