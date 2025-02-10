using Microsoft.AspNetCore.Mvc.ApplicationModels;
using MVC.Models;
using System.Buffers.Text;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MVC.Services
{
    public class CommonApiService
    {
        private const string BASE_URL = "https://localhost:7052/api/v1/";

        private readonly HttpClient _httpClient;
        public CommonApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public string StringFactory (string baseString, int page = 0, int pageSize = 0)
        {
            return $"{baseString}?page={page}&pageSize={pageSize}";
        }

        public async Task<ApiResponse<T>> GetApiResponseAsync<T>(string url)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(BASE_URL + url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<T>>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (apiResponse == null)
                {
                    return new ApiResponse<T>();
                }

                apiResponse.Content ??= new List<T>();
                apiResponse.IsSuccess = true;

                return apiResponse;
            }
            catch (Exception)
            {
                return new ApiResponse<T>();
            }
        }

        public async Task<ApiResponse<T>> PostApiReponseAsync<T>(string url, object data, string bearerToken)
        {
            try
            {
                string json = JsonSerializer.Serialize(data);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                HttpResponseMessage response = await _httpClient.PostAsync(BASE_URL + url, content);

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<T>>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (apiResponse == null)
                {
                    return new ApiResponse<T>();
                }

                apiResponse.Content ??= new List<T>();
                apiResponse.IsSuccess = true;

                return apiResponse;
            }
            catch (Exception)
            {
                return new ApiResponse<T>();
            }
        }

        public async Task<ApiResponse<T>> DeleteAsync<T>(string url, string bearerToken)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                HttpResponseMessage response = await _httpClient.DeleteAsync(BASE_URL + url);

                response.EnsureSuccessStatusCode();

                ApiResponse<T> apiResponse = new ApiResponse<T>
                {
                    IsSuccess = true
                };

                return apiResponse;
            }
            catch (Exception)
            {
                return new ApiResponse<T>();
            }
        }
    }
}
