using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Dto;
using WebApi.Models;
using WebApi.Repository;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PostController : Controller
    {
        private readonly IPostRepository _repository;
        public PostController(IPostRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("/api/v1/Category/{categoryId}/posts")]
        public async Task<IActionResult> GetPostsByCategoryId(string categoryId, int page = 0, int pageSize = 0)
        {
            if (!int.TryParse(categoryId, out int categoryInt))
            {
                return BadRequest("Invalid category id.");
            }

            PageInfo pageInfo = new PageInfo (pageSize, page);
            OperationResult result = await _repository.GetPostsByCategoryIdAsync(categoryInt, pageInfo);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return NotFound(result.ErrorMessage);
        }

        [HttpGet("/api/v1/User/{userId}/posts")]
        public IActionResult GetPostsByUserId(int userId, int page = 0, int pageSize = 0)
        {
            // TODO: Implement this method
            return Ok();
        }

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPostById(int postId)
        {
            if (postId <= 0)
            {
                return BadRequest("Invalid post id.");
            }

            OperationResult result = await _repository.GetPostByIdAsync(postId);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return NotFound(result.ErrorMessage);
        }

        [HttpPost(), Authorize]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDto createPostDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Invalid user credentials");
            }

            OperationResult result = await _repository.CreatePostAsync(createPostDto, userId);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.ErrorMessage);
        }

        [HttpPut()]
        public IActionResult UpdatePost(Post post)
        {
            // TODO: Implement this method
            return Ok();
        }

        [HttpDelete()]
        public IActionResult DeletePost(int postId)
        {
            // TODO: Implement this method
            return Ok();
        }
    }
}
