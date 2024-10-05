using System.Text.Json;
using Web2Lab1.Server.Interfaces;

namespace Web2Lab1.Server.Services
{
    public class AuthService:IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public AuthService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var clientId = _configuration["Auth0:ClientId"]!;
            var clientSecret = _configuration["Auth0:ClientSecret"]!;
            var domain = _configuration["Auth0:Domain"]!;
            var audience = _configuration["Auth0:Audience"]!;

            var tokenRequest = new Dictionary<string, string>
            {
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "audience", audience },
                { "grant_type", "client_credentials" }
            };

            var response = await _httpClient.PostAsync($"https://{domain}/oauth/token", new FormUrlEncodedContent(tokenRequest));
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            var token = JsonDocument.Parse(result).RootElement.GetProperty("access_token").GetString()!;
            return token;
        }
    }
}
