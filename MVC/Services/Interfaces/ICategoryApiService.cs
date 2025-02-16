using Common.Models;
using MVC.Models;

namespace MVC.Services.Interfaces
{
    public interface ICategoryApiService
    {
        public Task<Result<List<Category>>> GetCategoriesAsync();
        public Task<Result<Category>> GetCategoryByIdAsync(int id);
    }
}