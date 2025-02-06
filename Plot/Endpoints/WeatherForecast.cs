using Microsoft.AspNetCore.Mvc;

public class WeatherForecastResponse
{
    public DateTime Time { get; set; }
    public string? Summary { get; set; }
}

[ApiController]
[Route("/weatherforecast")]
public class WeatherForecastController : ControllerBase
{
    [HttpGet]
    public ActionResult<WeatherForecastResponse> GETWeatherForecast()
    {
        var weatherForecast = new WeatherForecastResponse
        {
            Time = DateTime.Now,
            Summary = "Rainy"
        };

        return Ok(weatherForecast);
    }
}