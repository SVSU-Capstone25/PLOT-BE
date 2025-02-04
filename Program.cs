using Microsoft.EntityFrameworkCore;
using Plot.Services;
using Plot.Context;
using Plot.Data.Models.Email;
using Plot.Data.Models.Token;


var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:8085");//"http://localhost:5000");


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//-----------------------------------------------------------------------------------------------------test

builder.Services.AddDbContext<PlotContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

builder.Services.Configure<TokenSettings>(
    builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddScoped<EmailService>();

builder.Services.AddSingleton<TokenService>();

builder.Services.AddControllers();


var app = builder.Build();


// Configure the HTTP request pipeline.

app.UseRouting();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();