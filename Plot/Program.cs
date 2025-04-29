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


EnvironmentSettings envSettings = new();
builder.Services.AddScoped<EnvironmentSettings>();

builder.WebHost.UseUrls(envSettings.issuer);

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

// Configure authentication settings for api endpoints that have the 
// [Authorize] tag. Uses jwt Bearer authentication. 
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

// Configure authorization settings 
//Defines role-based access for Employee, Manager, and Owner.
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Employee", policy => policy.RequireClaim("Role", "Owner", "Manager", "Employee"))
    .AddPolicy("Manager", policy => policy.RequireClaim("Role", "Owner", "Manager"))
    .AddPolicy("Owner", policy => policy.RequireClaim("Role", "Owner"));


// Add contexts and services as scoped services throughout
// the backend project for dependency injection.
builder.Services.AddSingleton<IAuthContext, AuthContext>();
builder.Services.AddSingleton<IUserContext, UserContext>();
builder.Services.AddSingleton<IStoreContext, StoreContext>();
builder.Services.AddSingleton<IFloorsetContext, FloorsetContext>();
builder.Services.AddSingleton<IFixtureContext, FixtureContext>();
builder.Services.AddSingleton<ISalesContext, SalesContext>();
builder.Services.AddScoped<ClaimParserService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<TokenService>();


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

// Middleware Configuration

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapHealthChecks("/health");
app.MapControllers();
app.Run();