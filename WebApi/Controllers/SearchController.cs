using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Repository.Interfaces;
using Common.Models;
using Common.Dto.Search;
using WebApi.Services.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v2/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;
        private readonly ILogger<SearchController> _logger;

        public SearchController(ISearchService searchService, ILogger<SearchController> logger)
        {
            _searchService = searchService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> SearchAsync(string searchString, int page = 0, int pageSize = 0)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return BadRequest("Invalid search query.");
            }

            PageInfo pageInfo = new PageInfo(page, pageSize);
            Result<PagedSearchResultDto> result = await _searchService.SearchAsync(searchString, pageInfo);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            _logger.LogError(result.ErrorMessage);
            return HandleErrors(result);
        }
    }
}
