using Common.Enums;
using Common.Dto.Comment;
using Common.Dto.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Models;
using Common.Models;
using WebApi.Services.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v2/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly ILogger<PostsController> _logger;
        public PostsController(IPostService postService, ILogger<PostsController> logger)
        {
            _postService = postService;
            _logger = logger;
        }

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPost(int postId, string? userId = null)
        {
            if (postId <= 0)
            {
                return BadRequest();
            }

            Result<PostDto> result = await _postService.GetPostAsync(postId, userId);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            _logger.LogError(result.ErrorMessage);
            return HandleErrors(result);
        }

        [HttpGet("/api/v2/categories/{categoryId}/posts")]
        public async Task<IActionResult> GetPostsByCategory(int categoryId, int page = 0, int pageSize = 0, SortBy sortBy = SortBy.Date, SortDirection sortDirection = SortDirection.Desc)
        {
            if (categoryId <= 0)
            {
                return BadRequest();
            }

            PageInfo pageInfo = new PageInfo (page, pageSize);
            Result<PagedPostsDto> result = await _postService.GetPostsByCategoryAsync(categoryId, pageInfo, sortBy, sortDirection);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            _logger.LogError(result.ErrorMessage);
            return HandleErrors(result);
        }

        [HttpGet("/api/v2/users/{username}/posts")]
        public async Task<IActionResult> GetPostsByUsername(string username, int page = 0, int pageSize = 0, SortBy sortBy = SortBy.Date, SortDirection sortDirection = SortDirection.Desc)
        
        {
            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized("Invalid user credentials.");
            }

            PageInfo pageInfo = new PageInfo(page, pageSize);
            Result<PagedPostsDto> result = await _postService.GetPostsByUsernameAsync(username, pageInfo);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            _logger.LogError(result.ErrorMessage);
            return HandleErrors(result);
        }

        // TODO: Implement input validation
        [Authorize]
        [HttpPost()]
        public async Task<IActionResult> CreatePost(CreatePostDto createPostDto)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Invalid user credentials.");
            }

            Result<PostDto> result = await _postService.CreatePostAsync(userId, createPostDto);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            _logger.LogError(result.ErrorMessage);
            return HandleErrors(result);
        }

        // TODO: Implement input validation
        [Authorize]
        [HttpPut("{postId}")]
        public async Task<IActionResult> UpdatePost(int postId, UpdatePostDto updatePostDto)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Invalid user credentials.");
            }

            Result<PostDto> result = await _postService.UpdatePostAsync(postId, userId, updatePostDto);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            _logger.LogError(result.ErrorMessage);
            return HandleErrors(result);
        }

        [Authorize]
        [HttpDelete("{postId}")]
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

            Result<PostDto> result = await _postService.DeletePostAsync(postId, userId);

            if (result.IsSuccess)
            {
                return Ok(result);
            }

            _logger.LogError(result.ErrorMessage);
            return HandleErrors(result);
        }
    }
}
