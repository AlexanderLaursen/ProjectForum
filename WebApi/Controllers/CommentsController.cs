using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Common.Dto.Comment;
using WebApi.Models;
using Common.Models;
using WebApi.Services.Interfaces;
using Microsoft.Extensions.Configuration.UserSecrets;
using Common.Dto.Post;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v2/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly ILogger<CommentsController> _logger;
        public CommentsController(ICommentService commentService, ILogger<CommentsController> logger)
        {
            _commentService = commentService;
            _logger = logger;
        }

        [HttpGet("{commentId}")]
        public async Task<IActionResult> GetComment(int commentId, string? userId = null)
        {
            if (commentId <= 0)
            {
                return BadRequest("Invalid comment id.");
            }

            Result<CommentDto> result = await _commentService.GetCommentAsync(commentId, userId);

            if (!result.IsSuccess)
            {
                return HandleErrors(result);
            }

            return Ok(result.Value);
        }

        [HttpGet("/api/v2/posts/{postId}/comments")]
        public async Task<IActionResult> GetCommentsByPostId(int postId, int page = 0, int pageSize = 0, string? userId = null)
        {
            if (postId <= 0)
            {
                return BadRequest("Invalid post id.");
            }

            PageInfo pageInfo = new PageInfo(page, pageSize);
            Result<PagedCommentsDto> result = await _commentService.GetCommentsByPostIdAsync(postId, pageInfo, userId);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            _logger.LogError(result.ErrorMessage);
            return HandleErrors(result);
        }

        [HttpGet("/api/v2/users/{username}/comments")]
        public async Task<IActionResult> GetCommentsByUsername(string username, int page = 0, int pageSize = 0)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Invalid username.");
            }

            PageInfo pageInfo = new PageInfo(page, pageSize);
            Result<PagedCommentsDto> result = await _commentService.GetCommentsByUsernameAsync(username, pageInfo);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            _logger.LogError(result.ErrorMessage);
            return HandleErrors(result);
        }

        [Authorize]
        [HttpPost()]
        public async Task<IActionResult> CreateComment(CreateCommentDto createCommentDto)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Invalid user credentials.");
            }

            Result<CommentDto> result = await _commentService.CreateCommentAsync(userId, createCommentDto);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            _logger.LogError(result.ErrorMessage);
            return HandleErrors(result);
        }

        [Authorize]
        [HttpPut("{commentId}")]
        public async Task<IActionResult> UpdateComment(int commentId, UpdateCommentDto updateCommentDto)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Invalid user credentials.");
            }

            Result<CommentDto> result = await _commentService.UpdateCommentAsync(commentId, userId, updateCommentDto);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            _logger.LogError(result.ErrorMessage);
            return HandleErrors(result);
        }

        [Authorize]
        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            if (commentId <= 0)
            {
                return BadRequest("Invalid comment id.");
            }

            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Invalid user credentials.");
            }

            Result<CommentDto> result = await _commentService.DeleteCommentAsync(commentId, userId);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            _logger.LogError(result.ErrorMessage);
            return HandleErrors(result);
        }
    }
}
