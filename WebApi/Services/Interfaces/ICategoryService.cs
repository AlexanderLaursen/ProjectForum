using Common.Dto.Category;
using Common.Models;

namespace WebApi.Services.Interfaces
{
    public interface ICategoryService
    {
        public Task<Result<CategoriesDto>> GetCategoriesAsync();
        public Task<Result<CategoryDto>> GetCategoryByIdAsync(int id);
    }
}