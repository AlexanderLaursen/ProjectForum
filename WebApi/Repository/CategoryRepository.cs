using Mapster;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using Common.Dto.Category;
using WebApi.Models;
using WebApi.Repository.Interfaces;
using Common.Models;
using Common.Enums;

namespace WebApi.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<CategoriesDto>> GetCategoriesAsync()
        {
            try
            {
                List<CategoryDto> categoriesResult = await _context.Categories
                    .Select(c => new CategoryDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                    })
                    .ToListAsync();

                CategoriesDto categoriesDto = new()
                {
                    Categories = categoriesResult,
                };

                return Result<CategoriesDto>.Success(categoriesDto);
            }
            catch (Exception ex)
            {
                return Result<CategoriesDto>.Failure(ex.Message, ResultStatus.Error);
            }
        }

        public async Task<Result<CategoryDto>> GetCategoryByIdAsync(int id)
        {
            try
            {
                CategoryDto? categoryResult = await _context.Categories
                    .Where(c => c.Id == id)
                    .Select(c => new CategoryDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                    })
                    .FirstOrDefaultAsync();

                if (categoryResult == null)
                {
                    return Result<CategoryDto>.NotFound();
                }

                return Result<CategoryDto>.Success(categoryResult);
            }
            catch (Exception ex)
            {
                return Result<CategoryDto>.Failure(ex.Message, ResultStatus.Error);
            }
        }
    }
}
