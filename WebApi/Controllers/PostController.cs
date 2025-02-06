using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Dto.Post;
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

        [HttpGet("/api/v1/Category/{categoryId}/posts")]
        public async Task<IActionResult> GetPostsByCategoryId(string categoryId, int page = 0, int pageSize = 0)
        {
            if (!int.TryParse(categoryId, out int categoryInt))
            {
                return BadRequest("Invalid category id.");
            }

            PageInfo pageInfo = new PageInfo (page, pageSize);
            OperationResult result = await _repository.GetPostsByCategoryIdAsync(categoryInt, pageInfo);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return NotFound(result.ErrorMessage);
        }

        [HttpGet("/api/v1/User/{username}/posts")]
        public async Task<IActionResult> GetPostsByUsername(string username, int page = 0, int pageSize = 0)
        {
            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized("Invalid user credentials.");
            }

            PageInfo pageInfo = new PageInfo(page, pageSize);
            OperationResult result = await _repository.GetPostsByUsernameAsync(username, pageInfo);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("{postId}/history")]
        public async Task<IActionResult> GetPostHistoryByPostId(int postId, int page = 0, int pageSize = 0)
        {
            if (postId <= 0)
            {
                return BadRequest("Invalid post id.");
            }

            OperationResult result = await _repository.GetPostHistoryByPostId(postId, new PageInfo(page, pageSize));

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return NotFound(result.ErrorMessage);
        }

        // TODO: Implement input validation
        [Authorize]
        [HttpPost()]
        public async Task<IActionResult> CreatePost(CreatePostDto createPostDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Invalid user credentials.");
            }

            OperationResult result = await _repository.CreatePostAsync(userId, createPostDto);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.ErrorMessage);
        }

        // TODO: Implement input validation
        [Authorize]
        [HttpPut()]
        public async Task<IActionResult> UpdatePost(UpdatePostDto updatePostDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Invalid user credentials.");
            }

            OperationResult result = await _repository.UpdatePostAsync(userId, updatePostDto);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return NotFound(result.ErrorMessage);
        }

        [Authorize]
        [HttpDelete()]
        public async Task<IActionResult> DeletePost(int postId)
        {
            if (postId <= 0)
            {
                return BadRequest("Invalid post id.");
            }

            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Invalid user credentials.");
            }

            OperationResult result = await _repository.DeletePostAsync(postId, userId);

            if (result.Success)
            {
                return Ok();
            }

            return BadRequest(result.ErrorMessage);
        }
    }
}
