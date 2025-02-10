using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using MVC.Models;
using MVC.Models.ViewModels;

namespace MVC.Services
{
    public class AuthService
    {
        private const string BASE_URL = "https://localhost:7052/";
        private const string LOGIN_ENDPOINT = "login";
        private const string REGISTER_ENDPOINT = "register";
        public bool IsLoggedIn;

        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LoginResponse> LoginAsync(LoginData loginData)
        {
            try
            {
                string contentString = JsonSerializer.Serialize(loginData);
                HttpContent contentHttp = new StringContent(contentString, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(BASE_URL + LOGIN_ENDPOINT, contentHttp);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
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

                    IsLoggedIn = true;

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

        public async Task<ApiResponse<object>> RegisterAsync(LoginData loginData)
        {
            try
            {
                string contentString = JsonSerializer.Serialize(loginData);
                HttpContent contentHttp = new StringContent(contentString, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(BASE_URL + REGISTER_ENDPOINT, contentHttp);

                if (response.IsSuccessStatusCode)
                {
                    return new ApiResponse<object>
                    {
                        IsSuccess = true
                    };
                }
            }
            catch
            {
                return new ApiResponse<object>
                {
                    IsSuccess = false
                };
            }

            return new ApiResponse<object>
            {
                IsSuccess = false
            };
        }
    }
}