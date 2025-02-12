using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;
using System.Security.Claims;
using WebApi.Dto;
using WebApi.Dto.Post;
using WebApi.Models;
using WebApi.Repository;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LikesController : Controller
    {
        private readonly ILikesRepository _likesRepository;
        private readonly IPostRepository _postRepository;

        public LikesController(ILikesRepository likesRepository, IPostRepository postRepository)
        {
            _likesRepository = likesRepository;
            _postRepository = postRepository;
        }

        [Authorize]
        [HttpPost("{postId}")]
        public async Task<IActionResult> LikePost(int postId)
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

            OperationResult postResult = await _postRepository.GetPostByIdAsync(postId, new PageInfo(0, 0));

            if (!postResult.Success)
            {
                return NotFound(postResult.ErrorMessage);
            }

            Post post = postResult.InternalData as Post;
            if (post == null)
            {
                return NotFound();
            }

            bool result = await _likesRepository.LikePostAsync(postId, userId);
            if (result)
            {
                return Ok(post);
            }

            return BadRequest("Failed to like post.");
        }
    }
}
