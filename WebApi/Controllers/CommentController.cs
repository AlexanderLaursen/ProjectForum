using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CommentController : Controller
    {
        [HttpGet()]
        public IActionResult GetCommentsByPostId(int postId)
        {
            return Ok();
        }

        [HttpPost()]
        public IActionResult CreateComment()
        {
            return Ok();
        }

        [HttpPut()]
        public IActionResult UpdateComment()
        {
            return Ok();
        }

        [HttpDelete()]
        public IActionResult DeleteComment(int commentId)
        {
            return Ok();
        }
    }
}
