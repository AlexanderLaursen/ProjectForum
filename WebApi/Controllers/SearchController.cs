using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Repository;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SearchController : Controller
    {
        private readonly ISearchRepository _searchRepository;

        public SearchController(ISearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
        }

        [HttpGet]
        public async Task<IActionResult> SearchAsync(string searchString, int page = 0, int pageSize = 0)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return BadRequest("Invalid search query.");
            }

            PageInfo pageInfo = new PageInfo(page, pageSize);

            var result = await _searchRepository.SearchAsync(searchString, pageInfo);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return NotFound(result.ErrorMessage);
        }
    }
}
