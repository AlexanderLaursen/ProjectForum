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
            // TODO: Implement this method
            return Ok();
        }

        [HttpGet("id")]
        public IActionResult GetCategoryById(int categoryId)
        {
            // TODO: Implement this method
            return Ok();
        }

        [HttpPost()]
        public IActionResult CreateCategory()
        {
            // TODO: Implement this method
            return Ok();
        }

        [HttpPut()]
        public IActionResult UpdateCategory()
        {
            // TODO: Implement this method
            return Ok();
        }

        [HttpDelete()]
        public IActionResult DeleteCategory(int categoryId)
        {
            // TODO: Implement this method
            return Ok();
        }
    }
}
