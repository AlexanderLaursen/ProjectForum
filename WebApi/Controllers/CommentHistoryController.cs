using Common.Dto.CommentHistory;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Repository.Interfaces;
using WebApi.Services.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v2/[controller]")]
    public class CommentHistoryController : ControllerBase
    {
        private readonly ICommentHistoryService _commentHistoryService;

        public CommentHistoryController(ICommentHistoryService commentHistoryService)
        {
            _commentHistoryService = commentHistoryService;
        }

        [HttpGet("/api/v2/comment-history/{commentId}")]
        public async Task<IActionResult> GetCommentHistory(int commentId, int page = 0, int pageSize = 0)
        {
            if (commentId <= 0)
            {
                return BadRequest();
            }

            PageInfo pageInfo = new PageInfo(page, pageSize);
            Result<CommentHistoriesDto> result = await _commentHistoryService.GetCommentHistoryAsync(commentId, pageInfo);

            if (!result.IsSuccess)
            {
                return HandleErrors(result);
            }

            return Ok(result.Value);
        }
    }
}
