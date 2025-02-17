using System.Text;
using System.Text.Json;
using Common.Dto.User;
using Common.Models;
using MVC.Helpers;
using MVC.Models;
using MVC.Repositories;
using MVC.Repositories.Interfaces;
using MVC.Services.Interfaces;

namespace MVC.Services
{
    public class AuthService : CommonApiService, IAuthService
    {
        private const string BASE_URL = "https://localhost:7052/";
        private const string LOGIN_ENDPOINT = "login";
        private const string REGISTER_ENDPOINT = "register";
        private const string USER_ENDPOINT = "User";
        public bool IsLoggedIn;

        private readonly HttpClient _httpClient;
        private readonly ApiRepository _commonApiService;
        private readonly IApiRepository _apiRepository;
        private readonly IAuthRepository _authRepository;

        public AuthService(HttpClient httpClient, ApiRepository commonApiService, IApiRepository apiRepository, IAuthRepository authRepository)
        {
            _httpClient = httpClient;
            _commonApiService = commonApiService;
            _apiRepository = apiRepository;
            _authRepository = authRepository;
        }

        public async Task<Result<BearerToken>> LoginAsync(LoginData loginData)
        {
            return await _authRepository.LoginAsync(loginData);
        }

        public async Task<LoginResponse> LoginAsyncOld(LoginData loginData)
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

                var response = await _commonApiService.GetApiResponseAsyncOld<string>(url);

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

        public async Task<Result<UserDto>> GetUserAsync(string userName)
        {
            return await _authRepository.GetUserAsync(userName);
        }
    }
}