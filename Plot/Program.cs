/*
    Filename: ClaimParserService.cs
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
using Xunit;

var builder = WebApplication.CreateBuilder(args);

EnvironmentSettings envSettings = new();
builder.Services.AddScoped<EnvironmentSettings>();

builder.WebHost.UseUrls(envSettings.issuer);

// Add CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins(envSettings.audience)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();

// Bind settings from appsettings.json to the EmailSettings model
// to be used to configure EmailService settings.
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = envSettings.issuer,
            ValidateAudience = true,
            ValidAudience = envSettings.audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(envSettings.secret_key)),
            ValidateLifetime = true,
        };
    });


builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Employee", policy => policy.RequireClaim("Role", "3", "2", "1"))
    .AddPolicy("Manager", policy => policy.RequireClaim("Role", "1", "2"))
    .AddPolicy("Owner", policy => policy.RequireClaim("Role", "1"));

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

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseRouting();
//app.UseHttpsRedirection();
app.UseCors("AllowLocalhost");

app.UseAuthentication();
app.UseAuthorization();
app.MapHealthChecks("/health");
app.MapControllers();
app.Run();