using Common.Models;
using Microsoft.AspNetCore.Mvc;
using MVC.Models.ViewModels;
using MVC.Services;

namespace MVC.Controllers
{
    public class SearchController : Controller
    {
        private readonly SearchService _searchService;

        public SearchController(SearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet("/Search")]
        public async Task<IActionResult> Index(string searchString, int page = 0, int pageSize = 0)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return View(new SearchViewModel());
            }

            PageInfo pageInfo = new PageInfo(page, pageSize);

            var result = await _searchService.SearchAsync(searchString, pageInfo);

            if (!result.IsSuccess)
            {
                return View("Error");
            }

            if (result.Content.Data.Count == 0)
            {
                return View(new SearchViewModel(searchString));
            }

            SearchViewModel viewModel = new SearchViewModel
            {
                SearchResults = result.Content.Data,
                PageInfo = result.Content.PageInfo,
                SearchString = searchString,
            };

            return View(viewModel);
        }
    }
}
