using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using System.Net.Http.Headers;
using Web2Lab1.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var authSettings = builder.Configuration.GetSection("Auth0");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://{authSettings["Domain"]}/";
        options.Audience = authSettings["Audience"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = $"https://{authSettings["Domain"]}/",
            ValidAudience = authSettings["Audience"]
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddHttpClient<AuthService>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
