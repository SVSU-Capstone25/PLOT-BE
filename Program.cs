using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Plot.Model;
using Plot.Services;
using Microsoft.Extensions.DependencyInjection;
var builder = WebApplication.CreateBuilder(args);


builder.WebHost.UseUrls("http://0.0.0.0:8085");

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


// Registers EmailSender class as a scoped service in the dependency injection container.

builder.Services.Configure<EmailSettingsModel>(
    builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddScoped<EmailService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    //TEST____________________
    using (var scope = app.Services.CreateScope())
    {
        var emailService = scope.ServiceProvider.GetRequiredService<EmailService>();
        await emailService.SendPasswordResetEmailAsync("Michael","mapolhill@gmail.com","https://yourwebsite.com/reset-password");
        //await emailSender.SendEmail("Test Receiver", "mapolhill@gmail.com", "Test Email", "This is a test email.");
        Console.WriteLine("Test email sent directly during debugging!");
    }
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}


