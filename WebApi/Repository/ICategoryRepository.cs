using WebApi.Models;

namespace WebApi.Repository
{
    public interface ICategoryRepository
    {
        public Task<OperationResult> GetAllCategoriesAsync();
        public Task<OperationResult> GetCategoryByIdAsync(int id);
    }
}