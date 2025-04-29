/*
    Filename: Program.cs
    Part of Project: PLOT/PLOT-BE/Plot

    Project Purpose: This project is the backend for Plato's Closet
    PLOT floorset allocation software, linking the frontend and the database
    together through backend API endpoints.

    Written by: SVSU 2025 Capstone Team
*/

using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Plot.Data.Models.Auth.Email;
using Plot.Data.Models.Env;
using Plot.DataAccess.Contexts;
using Plot.DataAccess.Interfaces;
using Plot.Services;

var builder = WebApplication.CreateBuilder(args);

// Add the environment settings from the .env file as a scoped service
// into the program service container.
EnvironmentSettings envSettings = new();
builder.Services.AddScoped<EnvironmentSettings>();

builder.WebHost.UseUrls(envSettings.issuer);

// Add the cross-origin middleware service into the program service container, allowing
// the frontend to talk to the backend.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .SetIsOriginAllowed(origin => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();


// Bind settings from appsettings.json to the EmailSettings model
// to be used to configure EmailService settings.
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

// Add the authentication middleware service into the program service container,
// adding Jwt bearer tokens into the program. The Jwt bearer token settings
// are based on the .env file values and seeing the implementation for the
// tokens, check the AuthController.cs, AuthContext.cs, and TokenService files.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        //Configure JWT validation
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = envSettings.issuer,
            ValidateAudience = true,
            ValidAudience = envSettings.audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(envSettings.auth_secret_key)),//Use secret key from env
            ValidateLifetime = true,
        };
    });

// Add the authorization middleware service into the program service container,
// allowing for policy authorization middleware. These will check the role
// claim tied to the auth bearer token appended to the http requests from the frontend
// and make sure that the specified role from the token is allowed to continue
// to the endpoint. To check implementations, look at the endpoints in the controllers
// folder and see the data annotations on top of each endpoint function or controller class.
// If the authorize data annotation doesn't have a policy tied to it, then it will
// only check to see if the token signature is valid.
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Employee", policy => policy.RequireClaim("Role", "Owner", "Manager", "Employee"))
    .AddPolicy("Manager", policy => policy.RequireClaim("Role", "Owner", "Manager"))
    .AddPolicy("Owner", policy => policy.RequireClaim("Role", "Owner"));

// Add the services and database context classes into the program service container.
// These will be injected as dependencies throughout the program.
builder.Services.AddSingleton<IAuthContext, AuthContext>();
builder.Services.AddSingleton<IUserContext, UserContext>();
builder.Services.AddSingleton<IStoreContext, StoreContext>();
builder.Services.AddSingleton<IFloorsetContext, FloorsetContext>();
builder.Services.AddSingleton<IFixtureContext, FixtureContext>();
builder.Services.AddSingleton<ISalesContext, SalesContext>();
builder.Services.AddScoped<ClaimParserService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<TokenService>();

// Add the endpoint controllers into the program and
// turn off the json property naming policy so the inputs/outputs
// don't get automatically converted to camel casing. 
// If you don't have the property naming policy as null,
// then the json modelling will try to convert EXAMPLE_DATA
// into camel case, which would go out to a messed up naming
// convention with the underscores like: Example_daTA.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.Use(async (context, next) =>
{
    context.Response.Headers.Append("Referrer-Policy", "no-referrer-when-downgrade");
    await next.Invoke();
});

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapHealthChecks("/health");
app.MapControllers();
app.Run();