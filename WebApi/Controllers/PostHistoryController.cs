using Microsoft.AspNetCore.Mvc;
using Common.Models;
using Common.Dto.PostHistory;
using WebApi.Services.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v2/[controller]")]
    public class PostHistoryController : ControllerBase
    {
        private readonly IPostHistoryService _postHistoryService;

        public PostHistoryController(IPostHistoryService postHistoryService)
        {
            _postHistoryService = postHistoryService;
        }

        [HttpGet("/api/v2/post-history/{postId}")]
        public async Task<IActionResult> GetPostHistory(int postId, int page = 0, int pageSize = 0)
        {
            if (postId <= 0)
            {
                return BadRequest();
            }

            PageInfo pageInfo = new PageInfo(page, pageSize);
            Result<PostHistoriesDto> result = await _postHistoryService.GetPostHistoryAsync(postId, pageInfo);

            if (!result.IsSuccess)
            {
                return HandleErrors(result);
            }

            return Ok(result.Value);
        }
    }
}
