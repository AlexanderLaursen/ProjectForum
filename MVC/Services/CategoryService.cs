using MVC.Models;
using System.Text.Json;

namespace MVC.Services
{
    public class CategoryService
    {
        private const string PREFIX = "Category";
        private readonly HttpClient _httpClient;
        private readonly CommonApiService _commonApiService;
        public CategoryService(HttpClient httpClient, CommonApiService commonApiService)
        {
            _httpClient = httpClient;
            _commonApiService = commonApiService;
        }

        public async Task<ApiResponse<Category>> GetCategoriesAsync()
        {
            return await _commonApiService.GetApiResponseAsync<Category>(PREFIX);
        }

        public async Task<ApiResponse<Category>> GetCategoryByIdAsync(int id)
        {
            if(id <= 0)
            {
                return new ApiResponse<Category>();
            }

            return await _commonApiService.GetApiResponseAsync<Category>($"{PREFIX}/{id}");
        }
    }
}
