using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;
using System.Security.Claims;
using Common.Dto;
using Common.Dto.Post;
using WebApi.Models;
using WebApi.Repository;
using WebApi.Services.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LikesController : Controller
    {
        private readonly ILikesService _likesService;

        public LikesController(ILikesService likesService)
        {
            _likesService = likesService;
        }

        [Authorize]
        [HttpPost("post/{postId}")]
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

            OperationResultNew<PostLike> result = await _likesService.LikePostAsync(postId, userId);
            if (result.Success)
            {
                return Ok();
            }

            return BadRequest("Failed to like post.");
        }

        [Authorize]
        [HttpDelete("post/{postId}")]
        public async Task<IActionResult> DeletePostLikeAsync(int postId)
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

            OperationResultNew<PostLike> result = await _likesService.DeletePostLikeAsync(postId, userId);

            if (result.Success)
            {
                return Ok();
            }

            return BadRequest();
        }

        [Authorize]
        [HttpPost("comment/{commentId}")]
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

            OperationResultNew<CommentLike> result = await _likesService.LikeCommentAsync(commentId, userId);

            if (result.Success)
            {
                return Ok();
            }

            return BadRequest();
        }

        [Authorize]
        [HttpDelete("comment/{commentId}")]
        public async Task<IActionResult> DeleteCommentLikeAsync(int commentId)
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
            OperationResultNew<CommentLike> result = await _likesService.DeleteCommentLikeAsync(commentId, userId);
            if (result.Success)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
