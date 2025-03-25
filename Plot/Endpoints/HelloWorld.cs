using Microsoft.AspNetCore.Mvc;

public class HelloWorldResponse
{
    public required string Hello { get; set; }
}

[ApiController]
[Route("/helloworld")]
public class HelloWorldController : ControllerBase
{
    [HttpGet]
    public ActionResult<HelloWorldResponse> GETWeatherForecast()
    {
        var helloWorld = new HelloWorldResponse
        {
            Hello = "World"
        };

        return Ok(helloWorld);
    }
}