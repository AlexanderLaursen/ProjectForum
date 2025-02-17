using Microsoft.AspNetCore.Mvc;
using MVC.Models.ViewModels;
using MVC.Models;
using MVC.Services;
using Common.Models;
using System.Runtime.CompilerServices;
using MVC.Services.Interfaces;

namespace MVC.Controllers
{
    public class PostHistoryController : Controller
    {
        private readonly IPostHistoryApiService _postHistoryService;
        private readonly ILogger<PostHistoryController> _logger;
        public PostHistoryController(IPostHistoryApiService postHistoryService, ILogger<PostHistoryController> logger)
        {
            _postHistoryService = postHistoryService;
            _logger = logger;
        }

        [HttpGet("/post-history/{postId}")]
        public async Task<IActionResult> GetPostHistory(int postId, int page, int pageSize)
        {
            if (postId <= 0)
            {
                return BadRequest();
            }

            PageInfo pageInfo = new PageInfo(page, pageSize);
            Result<PostHistories> result = await _postHistoryService.GetPostHistoryAsync(postId, pageInfo);

            if (!result.IsSuccess && result.Value != null)
            {
                PostHistoryViewModel viewModel = new PostHistoryViewModel
                {
                    Post = result.Value.Post,
                    PostHistory = result.Value.PostHistory,
                    PageInfo = result.Value.PageInfo
                };

                return View(viewModel);
            }

            _logger.LogError(result.ErrorMessage);
            return View("Error", new ErrorViewModel());
        }
    }
}
