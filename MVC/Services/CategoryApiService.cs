using Common.Dto.Category;
using Common.Models;
using Mapster;
using MVC.Models;
using MVC.Repositories;
using MVC.Repositories.Interfaces;
using MVC.Services.Interfaces;

namespace MVC.Services
{
    public class CategoryApiService : CommonApiService, ICategoryApiService
    {
        private const string PREFIX = "categories";
        private readonly IApiRepository _apiRepository;
        public CategoryApiService(IApiRepository apiRepository)
        {
            _apiRepository = apiRepository;
        }

        public async Task<Result<List<Category>>> GetCategoriesAsync()
        {
            Result<CategoriesDto> result = await _apiRepository.GetAsync<CategoriesDto>(UrlFactory(PREFIX));

            if (result.IsSuccess && result.Value == null)
            {
                    return Result<List<Category>>.NotFound();
            }

            return ConvertDtoList<CategoriesDto, CategoryDto, Category>(result, result.Value?.Categories!);
        }

        public async Task<Result<Category>> GetCategoryByIdAsync(int id)
        {
            string url = UrlFactory($"{PREFIX}/{id.ToString()}");

            Result<CategoryDto> result = await _apiRepository.GetAsync<CategoryDto>(url);

            Result<Category> category = ConvertDto<CategoryDto, Category>(result);

            return category;
        }
    }
}
