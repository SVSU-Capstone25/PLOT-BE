using Microsoft.EntityFrameworkCore;
using Plot.Services;
using Plot.Context;
using Plot.Data.Models.Email;
using Plot.Data.Models.Token;

//TEMP COMMENT:Im just adding comments for my additions since this file will most likely be modified until projects completion


var builder = WebApplication.CreateBuilder(args);


builder.WebHost.UseUrls("http://0.0.0.0:8085");


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configure the Entity Framework context to manipulate the database.
// Sets the options for the context to use SqlServer and the servers 
// connection.
builder.Services.AddDbContext<PlotContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")));


// Bind settings from appsettings.json to the EmailSettings model
// to be used to configure EmailService settings.
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

// Bind settings from appsettings.json to the TokenSettings model
// to be used to configure TokenService settings.
builder.Services.Configure<TokenSettings>(
    builder.Configuration.GetSection("JwtSettings"));

// Add EmailService as a scoped service
// allows for settings to be used when injected.
builder.Services.AddScoped<EmailService>();

// Add TokenService as a scoped service
// allows for settings to be used when injected.
builder.Services.AddScoped<TokenService>();

// Add controllers to handle API endpoints.
builder.Services.AddControllers();

// Build the application and place into local variable for
// HTTP request configuration and to run.
var app = builder.Build();



// Use routings for API endpoints.
app.UseRouting();

// Redirects all HTTP to HTTPS for security.
app.UseHttpsRedirection();

// Map controllers to handle HTTP requests
app.MapControllers();

// Start the application.
app.Run();