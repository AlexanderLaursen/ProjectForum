using Common.Enums;
using Common.Dto.Comment;
using Common.Dto.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Models;
using WebApi.Repository.Interfaces;
using WebApi.Models.Ope;
using Common.Models;

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

        [Authorize]
        [HttpGet("{postId}/details")]
        public async Task<IActionResult> GetPostDetails(int postId, int page = 0, int pageSize = 0)
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

            PageInfo pageInfo = new PageInfo(page, pageSize);
            OperationResultNew result = await _repository.GetPostDetailsAsync(postId, userId, pageInfo);
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return NotFound(result.ErrorMessage);
        }

        [Authorize]
        [HttpGet("test2")]
        public async Task<IActionResult> Test()
        {
            return Ok("Test");
        }


        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPostById(int postId, int page = 0, int pageSize = 0)
        {
            if (postId <= 0)
            {
                return BadRequest("Invalid post id.");
            }

            PageInfo pageInfo = new PageInfo(page, pageSize);
            OperationResult result = await _repository.GetPostByIdAsync(postId, pageInfo);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return NotFound(result.ErrorMessage);
        }

        [HttpGet("/api/v1/Category/{categoryId}/posts")]
        public async Task<IActionResult> GetPostsByCategoryId(int categoryId, int page = 0, int pageSize = 0, SortBy sortBy = SortBy.Date, SortDirection sortDirection = SortDirection.Desc)
        {
            if (categoryId <= 0)
            {
                return BadRequest();
            }

            PageInfo pageInfo = new PageInfo (page, pageSize);
            OperationResult result = await _repository.GetPostsByCategoryIdAsync(categoryId, pageInfo, sortBy, sortDirection);

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
