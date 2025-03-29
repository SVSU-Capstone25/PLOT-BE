using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;

namespace Plot.Tests;

// public class WeatherForecastTests : IClassFixture<WebApplicationFactory<Program>>
// {
//     private readonly HttpClient _client;

//     public WeatherForecastTests(WebApplicationFactory<Program> factory)
//     {
//         _client = factory.CreateClient();
//     }

//     [Fact]
//     public async Task GET_WeatherForecast()
//     {
//         var response = await _client.GetAsync("/weatherforecast");

//         Assert.True(response.IsSuccessStatusCode);

//         var body = await response.Content.ReadFromJsonAsync<WeatherForecastResponse>();

//         Assert.NotNull(body);
//         Assert.Equal("Rainy", body.Summary);
//     }
// }
