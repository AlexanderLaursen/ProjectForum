using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Common.Dto.Comment;
using WebApi.Models;
using Common.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CommentController : Controller
    {
        private readonly ICommentRepository _repository;
        public CommentController(ICommentRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{commentId}")]
        public async Task<IActionResult> GetCommentById(int commentId)
        {
            if (commentId <= 0)
            {
                return BadRequest("Invalid comment id.");
            }

            OperationResult result = await _repository.GetCommentByIdAsync(commentId);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return NotFound(result.ErrorMessage);
        }

        [HttpGet("/api/v1/Post/{postId}/comments")]
        public async Task<IActionResult> GetCommentsByPostId(int postId, int page = 0, int pageSize = 0)
        {
            if (postId <= 0)
            {
                return BadRequest("Invalid post id.");
            }

            PageInfo pageInfo = new PageInfo(page, pageSize);
            OperationResult result = await _repository.GetCommentsByPostIdAsync(postId, pageInfo);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return NotFound(result.ErrorMessage);
        }

        [HttpGet("/api/v1/User/{username}/comments")]
        public async Task<IActionResult> GetCommentsByUsername(string username, int page = 0, int pageSize = 0)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Invalid username.");
            }

            PageInfo pageInfo = new PageInfo(page, pageSize);
            OperationResult result = await _repository.GetCommentsByUsernameAsync(username, pageInfo);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return NotFound(result.ErrorMessage);
        }

        [HttpGet("{commentId}/history")]
        public async Task<IActionResult> GetCommentHistoryById(int commentId, int page = 0, int pageSize = 0)
        {
            if (commentId <= 0)
            {
                return BadRequest("Invalid comment id.");
            }

            OperationResult result = await _repository.GetCommentHistoryById(commentId, new PageInfo(page, pageSize));

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return NotFound(result.ErrorMessage);
        }

        [Authorize]
        [HttpPost()]
        public async Task<IActionResult> CreateComment(CreateCommentDto createCommentDto)
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

            OperationResult result = await _repository.CreateCommentAsync(userId, createCommentDto);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return NotFound(result.ErrorMessage);
        }

        [Authorize]
        [HttpPut()]
        public async Task<IActionResult> UpdateComment(UpdateCommentDto updateCommentDto)
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

            OperationResult result = await _repository.UpdateCommentAsync(userId, updateCommentDto);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return NotFound(result.ErrorMessage);
        }

        [Authorize]
        [HttpDelete()]
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

            OperationResult result = await _repository.DeleteCommentAsync(commentId, userId);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return NotFound(result.ErrorMessage);
        }
    }
}
