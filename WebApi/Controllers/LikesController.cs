using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;
using System.Security.Claims;
using Common.Dto;
using Common.Dto.Post;
using WebApi.Models;
using WebApi.Repository;
using WebApi.Services.Interfaces;
using Common.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v2/[controller]")]
    public class LikesController : ControllerBase
    {
        private readonly ILikesService _likesService;
        private readonly ILogger<LikesController> _logger;

        public LikesController(ILikesService likesService, ILogger<LikesController> logger)
        {
            _likesService = likesService;
            _logger = logger;
        }

        [Authorize]
        [HttpPost("posts/{postId}")]
        public async Task<IActionResult> LikePostAsync(int postId)
        {
            if (postId <= 0)
            {
                return BadRequest("Invalid post id.");
            }

            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            Result<PostLike> result = await _likesService.LikePostAsync(postId, userId);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            _logger.LogError("Failed to like post.");
            return BadRequest("Failed to like post.");
        }

        [Authorize]
        [HttpDelete("posts/{postId}")]
        public async Task<IActionResult> RemovePostLikeAsync(int postId)
        {
            if (postId <= 0)
            {
                return BadRequest();
            }

            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            Result<PostLike> result = await _likesService.RemovePostLikeAsync(postId, userId);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            _logger.LogError("Failed to remove like post.");
            return BadRequest("Failed to remove like post.");
        }

        [Authorize]
        [HttpPost("comments/{commentId}")]
        public async Task<IActionResult> LikeCommentAsync(int commentId)
        {
            if (commentId <= 0)
            {
                return BadRequest("Invalid comment id.");
            }

            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            Result<CommentLike> result = await _likesService.LikeCommentAsync(commentId, userId);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            _logger.LogError("Failed to like comment.");
            return BadRequest("Failed to like comment.");
        }

        [Authorize]
        [HttpDelete("comments/{commentId}")]
        public async Task<IActionResult> RemoveCommentLikeAsync(int commentId)
        {
            if (commentId <= 0)
            {
                return BadRequest();
            }
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            Result<CommentLike> result = await _likesService.RemoveCommentLikeAsync(commentId, userId);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            _logger.LogError("Failed to remove comment like.");
            return BadRequest("Failed to remove comment like.");
        }
    }
}
