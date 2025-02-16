using System.Text;
using System.Text.Json;
using MVC.Models;

namespace MVC.Services
{
    public class AuthService
    {
        private const string BASE_URL = "https://localhost:7052/";
        private const string LOGIN_ENDPOINT = "login";
        private const string REGISTER_ENDPOINT = "register";
        private const string USER_ENDPOINT = "User";
        public bool IsLoggedIn;

        private readonly HttpClient _httpClient;
        private readonly CommonApiService _commonApiService;

        public AuthService(HttpClient httpClient, CommonApiService commonApiService)
        {
            _httpClient = httpClient;
            _commonApiService = commonApiService;
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

        public async Task<ApiResponseOld<object>> RegisterAsync(LoginData loginData)
        {
            try
            {
                string contentString = JsonSerializer.Serialize(loginData);
                HttpContent contentHttp = new StringContent(contentString, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(BASE_URL + REGISTER_ENDPOINT, contentHttp);

                if (response.IsSuccessStatusCode)
                {
                    return new ApiResponseOld<object>
                    {
                        IsSuccess = true
                    };
                }
            }
            catch
            {
                return new ApiResponseOld<object>
                {
                    IsSuccess = false
                };
            }

            return new ApiResponseOld<object>
            {
                IsSuccess = false
            };
        }

        public async Task<ApiResponseOld<string>> GetUserIdByUsernameAsync(string username)
        {
            try
            {
                string contentString = JsonSerializer.Serialize(new { Username = username });
                HttpContent contentHttp = new StringContent(contentString, Encoding.UTF8, "application/json");

                string url = $"{USER_ENDPOINT}/{username}/id";

                var response = await _commonApiService.GetApiResponseAsync<string>(url);

                if (response.IsSuccess)
                {
                    return response;
                }

                return new ApiResponseOld<string>
                {
                    Content = response.Content,
                    IsSuccess = false
                };
            }
            catch
            {
                return new ApiResponseOld<string>
                {
                    IsSuccess = false
                };
            }
        }
    }
}