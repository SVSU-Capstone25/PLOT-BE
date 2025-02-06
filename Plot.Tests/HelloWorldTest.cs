using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;

namespace Plot.Tests;

public class HelloWorldTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public HelloWorldTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GET_HelloWorld()
    {
        var response = await _client.GetAsync("/helloworld");

        Assert.True(response.IsSuccessStatusCode);

        var body = await response.Content.ReadFromJsonAsync<HelloWorldResponse>();

        Assert.NotNull(body);
        Assert.Equal("World", body.Hello);
    }
}
