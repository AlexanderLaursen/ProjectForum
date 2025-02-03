using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PostController : Controller
    {
        [HttpGet("/api/v1/Category/{categoryName}")]
        public IActionResult GetPostsByCategoryId(string categoryname)
        {
            return Ok();
        }

        [HttpGet("id")]
        public IActionResult GetPostById(int postId)
        {
            return Ok();
        }

        [HttpPost()]
        public IActionResult CreatePost(Post post)
        {
            return Ok();
        }

        [HttpPut()]
        public IActionResult UpdatePost(Post post)
        {
            return Ok();
        }

        [HttpDelete()]
        public IActionResult DeletePost(int postId)
        {
            return Ok();
        }
    }
}
