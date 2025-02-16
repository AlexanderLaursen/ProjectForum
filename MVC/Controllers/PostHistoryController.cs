using Microsoft.AspNetCore.Mvc;
using MVC.Models.ViewModels;
using MVC.Models;
using MVC.Services;
using Common.Models;

namespace MVC.Controllers
{
    public class PostHistoryController : Controller
    {
        private readonly PostHistoryService _postHistoryService;
        private readonly PostService _postService;
        public PostHistoryController(PostHistoryService postHistoryService, PostService postService)
        {
            _postHistoryService = postHistoryService;
            _postService = postService;
        }

        [HttpGet("/PostHistory/{postId}")]
        public async Task<IActionResult> GetPostHistory(int postId, int page = 0, int pageSize = 0)
        {
            if (postId <= 0)
            {
                return BadRequest();
            }

            PageInfo pageInfo = new PageInfo(page, pageSize);

            ApiResponseOld<Post> post = await _postService.GetPostByIdAsync(postId, pageInfo);

            if (!post.IsSuccess)
            {
                return NotFound();
            }

            ApiResponseOld<PostHistory> result = await _postHistoryService.GetPostHistoryByIdAsync(postId, pageInfo);

            if (!result.IsSuccess)
            {
                return NotFound();
            }

            PostHistoryViewModel viewModel = new()
            {
                Post = post.Content[0],
                PostHistory = result.Content,
                PageInfo = result.PageInfo
            };

            return View(viewModel);
        }
    }
}
