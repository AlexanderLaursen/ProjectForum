using System.Net.Http;
using System.Text;
using System.Text.Json;
using MVC.Models;
using MVC.Models.ViewModels;

namespace MVC.Services
{
    public class LoginService
    {
        private const string BASE_URL = "https://localhost:7052/";

        private readonly HttpClient _httpClient;

        public LoginService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LoginResponse> LoginAsync(LoginData loginData)
        {
            try
            {
                string contentString = JsonSerializer.Serialize(loginData);
                HttpContent contentHttp = new StringContent(contentString, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("https://localhost:7052/login", contentHttp);


                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var token = JsonSerializer.Deserialize<BearerToken>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (token == null)
                    {
                        return new LoginResponse
                        {
                            IsSuccess = false,
                        };
                    }

                    LoginResponse loginResponse = new();
                    loginResponse.IsSuccess = true;
                    loginResponse.Token = token;

                    return loginResponse;
                }
            }
            catch
            {
                return new LoginResponse
                {
                    IsSuccess = false,
                };
            }

            return new LoginResponse
            {
                IsSuccess = false,
            };
        }
    }
}