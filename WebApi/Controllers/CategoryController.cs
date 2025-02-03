using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CategoryController : Controller
    {
        [HttpGet()]
        public IActionResult GetCategories()
        {
            return Ok();
        }

        [HttpGet("id")]
        public IActionResult GetCategoryById(int categoryId)
        {
            return Ok();
        }

        [HttpPost()]
        public IActionResult CreateCategory()
        {
            return Ok();
        }

        [HttpPut()]
        public IActionResult UpdateCategory()
        {
            return Ok();
        }

        [HttpDelete()]
        public IActionResult DeleteCategory(int categoryId)
        {
            return Ok();
        }
    }
}
