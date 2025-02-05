using MVC.Models;
using System.Buffers.Text;
using System.Net.Http;
using System.Text.Json;

namespace MVC.Services
{
    public class CommonApiService
    {
        private const string BASE_URL = "https://localhost:7052/api/v1/";

        private readonly HttpClient _httpClient;
        public CommonApiService (HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<T>> GetApiResponse<T>(string url)
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
    }
}
