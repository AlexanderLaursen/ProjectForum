using Mapster;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Dto.Category;
using WebApi.Models;

namespace WebApi.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<OperationResult> GetAllCategoriesAsync()
        {
            try
            {

                List<Category> categories = await _context.Categories.ToListAsync();
                List<CategoryDto> categoriesDtos = categories.Adapt<List<CategoryDto>>();

                return new OperationResult
                {
                    Success = true,
                    Data = new Dictionary<string, object>
                    {
                        { "content", categoriesDtos }
                    }
                };
            }
            catch (Exception ex)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = $"{ex.Message} - {ex.InnerException?.ToString()}"
                };
            }
        }

        public async Task<OperationResult> GetCategoryByIdAsync(int id)
        {
            try
            {
                Category? category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
                if (category == null)
                {
                    return new OperationResult
                    {
                        Success = false,
                        ErrorMessage = "Category not found."
                    };
                }

                CategoryDto categoryDto = category.Adapt<CategoryDto>();

                List<CategoryDto> categoryDtoList = [categoryDto];

                return new OperationResult
                {
                    Success = true,
                    Data = new Dictionary<string, object>
                    {
                        { "content", categoryDtoList }
                    }
                };
            }
            catch (Exception ex)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = $"{ex.Message} - {ex.InnerException?.ToString()}"
                };
            }

        }
    }
}
