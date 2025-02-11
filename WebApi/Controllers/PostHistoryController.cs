using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Repository;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PostHistoryController : Controller
    {
        private readonly IPostHistoryRepository _postHistoryRepository;

        public PostHistoryController(IPostHistoryRepository postHistoryRepository)
        {
            _postHistoryRepository = postHistoryRepository;
        }

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPostHistory(int postId, int page = 0, int pageSize = 0)
        {
            if (postId <= 0)
            {
                return BadRequest();
            }

            PageInfo pageInfo = new PageInfo(page, pageSize);

            OperationResult result = await _postHistoryRepository.GetPostHistoryByIdAsync(postId, pageInfo);
            if (!result.Success)
            {
                return NotFound(result.ErrorMessage);
            }

            return Ok(result.Data);
        }
    }
}
