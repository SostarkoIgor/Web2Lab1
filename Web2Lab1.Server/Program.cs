using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using System.Net.Http.Headers;
using Web2Lab1.Server.Services;
using Web2Lab1.Server.Data;
using Microsoft.EntityFrameworkCore;
using Web2Lab1.Server.Interfaces;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.HttpsPolicy;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var auth0Domain = builder.Configuration["Auth0:Domain"];
var auth0ClientId = builder.Configuration["Auth0:ClientId"];
var auth0ClientSecret = builder.Configuration["Auth0:ClientSecret"];
var auth0Audience = builder.Configuration["Auth0:Audience"];

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Cookies";
    options.DefaultSignInScheme = "Cookies";
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie("Cookies")
.AddOpenIdConnect("Auth0", options =>
{
    options.Authority = $"https://{auth0Domain}";
    options.ClientId = auth0ClientId;
    options.ClientSecret = auth0ClientSecret;
    options.ResponseType = "code";
    options.SaveTokens = true;

    options.Scope.Clear();
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("email");

    options.CallbackPath = new PathString("/callback");
    options.ClaimsIssuer = "Auth0";
})
.AddJwtBearer(options =>
{
    options.Authority = $"https://{auth0Domain}/";
    options.Audience = auth0Audience;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = $"https://{auth0Domain}/",
        ValidateAudience = true,
        ValidAudience = auth0Audience,
        ValidateLifetime = true,
        IssuerSigningKeyResolver = (token, securityToken, kid, parameters) =>
        {
            var client = new HttpClient();
            var keyUri = $"https://{auth0Domain}/.well-known/jwks.json";
            var keys = client.GetFromJsonAsync<JsonWebKeySet>(keyUri).Result;
            return keys.Keys;
        }
    };
});

builder.Services.Configure<HttpsRedirectionOptions>(options =>
{
    options.HttpsPort = 443;
});

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("https://web2lab1-frontend.onrender.com/")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

builder.Services.AddHttpClient<AuthService>();

builder.Services.AddScoped<ITicketService, TicketService>();

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

app.UseCors();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
