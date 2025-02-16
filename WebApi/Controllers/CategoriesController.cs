using Microsoft.AspNetCore.Mvc;
using WebApi.Services.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v2/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService repository)
        {
            _categoryService = repository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var result = await _categoryService.GetCategoryByIdAsync(id);

            if (!result.IsSuccess)
            {
                return HandleErrors(result);
            }

            return Ok(result.Value);
        }

        [HttpGet()]
        public async Task<IActionResult> GetCategories()
        {
            var result = await _categoryService.GetCategoriesAsync();

            if (!result.IsSuccess)
            {
                return HandleErrors(result);
            }

            return Ok(result.Value);
        }
    }
}
