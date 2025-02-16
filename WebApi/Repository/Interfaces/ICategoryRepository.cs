using Common.Dto.Category;
using Common.Models;
using WebApi.Models;

namespace WebApi.Repository.Interfaces
{
    public interface ICategoryRepository
    {
        public Task<Result<CategoriesDto>> GetCategoriesAsync();
        public Task<Result<CategoryDto>> GetCategoryByIdAsync(int id);
    }
}