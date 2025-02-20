using Microsoft.EntityFrameworkCore;
using Plot.Services;
using Plot.Context;
using Plot.Data.Models.Email;
using Plot.Data.Models.Token;
using DotNetEnv;

//TEMP COMMENT:Im just adding comments for my additions since this file will most likely be modified until projects completion

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:500");//Changed to local host for testing

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Load in env file, so env variables can be used.
Env.Load();


// Get the connection string directly from environment variables
// throw NullException if not set.
string connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION") 
    ?? throw new ArgumentNullException(
        Environment.GetEnvironmentVariable("DB_CONNECTION"));

// Configure the Entity Framework context to manipulate the database.
// Sets the options for the context to use SqlServer and the servers 
// connection.
builder.Services.AddDbContext<PlotContext>(options => options.UseSqlServer(
    connectionString));



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