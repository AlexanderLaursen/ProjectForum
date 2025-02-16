using Common.Models;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Repository.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CommentHistoryController : Controller
    {
        private readonly ICommentHistoryRepository _commentHistoryRepository;

        public CommentHistoryController(ICommentHistoryRepository commentHistoryRepository)
        {
            _commentHistoryRepository = commentHistoryRepository;
        }

        [HttpGet("{commentId}")]
        public async Task<IActionResult> GetCommentHistoryById(int commentId, int page = 0, int pageSize = 0)
        {
            if (commentId <= 0)
            {
                return BadRequest();
            }

            PageInfo pageInfo = new PageInfo(page, pageSize);

            OperationResult result = await _commentHistoryRepository.GetCommentHistoryByIdAsync(commentId, pageInfo);

            if (!result.Success)
            {
                return NotFound(result.ErrorMessage);
            }

            return Ok(result.Data);
        }
    }
}
