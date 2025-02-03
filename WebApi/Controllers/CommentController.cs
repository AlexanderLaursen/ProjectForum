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
            // TODO: Implement this method
            return Ok();
        }

        [HttpGet("/api/v1/User/{userId}/comments")]
        public IActionResult GetCommentsByUserId(int userId)
        {
            // TODO: Implement this method
            return Ok();
        }

        [HttpPost()]
        public IActionResult CreateComment()
        {
            // TODO: Implement this method
            return Ok();
        }

        [HttpPut()]
        public IActionResult UpdateComment()
        {
            // TODO: Implement this method
            return Ok();
        }

        [HttpDelete()]
        public IActionResult DeleteComment(int commentId)
        {
            // TODO: Implement this method
            return Ok();
        }
    }
}
