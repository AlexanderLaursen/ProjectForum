using MVC.Models;

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
            var test = _httpClient.GetAsync("https://localhost:7052/api/v1/Category");
            Console.WriteLine(test);
            var categories = await _httpClient.GetFromJsonAsync<List<Category>>("https://localhost:7052/api/v1/Category");
            return categories ?? new List<Category>();
        }
    }
}
