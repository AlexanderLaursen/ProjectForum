using MVC.Models;
using System.Text.Json;

namespace MVC.Services
{
    public class CategoryService
    {
        private readonly HttpClient _httpClient;
        public CategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Category>> GetCategories()
        {
            var response = await _httpClient.GetAsync("https://localhost:7052/api/v1/Category");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return apiResponse.Content ?? new List<Category>();
        }

        public class ApiResponse
        {
            public List<Category> Content { get; set; }
        }

    }
}
