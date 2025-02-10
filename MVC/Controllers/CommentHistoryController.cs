using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Models.ViewModels;
using MVC.Services;

namespace MVC.Controllers
{
    public class CommentHistoryController : Controller
    {
        private readonly CommentHistoryService _commentHistoryService;
        public CommentHistoryController(CommentHistoryService commentHistoryService)
        {
            _commentHistoryService = commentHistoryService;
        }

        [HttpGet("{commentId}")]
        public async Task<IActionResult> GetCommentHistory(int commentId, int page = 0, int pageSize = 0)
        {
            if (commentId <= 0)
            {
                return BadRequest();
            }

            PageInfo pageInfo = new PageInfo(page, pageSize);

            ApiResponse<CommentHistory> result = await _commentHistoryService.GetCommentHistoryByIdAsync(commentId, pageInfo);

            if (!result.IsSuccess)
            {
                return NotFound();
            }

            CommentHistoryViewModel viewModel = new CommentHistoryViewModel
            {
                CommentHistory = result.Content,
                PageInfo = pageInfo
            };

            return View(viewModel);
        }
    }
}
