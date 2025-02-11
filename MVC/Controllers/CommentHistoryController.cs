using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Models.ViewModels;
using MVC.Services;

namespace MVC.Controllers
{
    public class CommentHistoryController : Controller
    {
        private readonly CommentHistoryService _commentHistoryService;
        private readonly CommentService _commentService;
        public CommentHistoryController(CommentHistoryService commentHistoryService, CommentService commentService)
        {
            _commentHistoryService = commentHistoryService;
            _commentService = commentService;
        }

        [HttpGet("/CommentHistory/{commentId}")]
        public async Task<IActionResult> GetCommentHistory(int commentId, int page = 0, int pageSize = 0)
        {
            if (commentId <= 0)
            {
                return BadRequest();
            }

            PageInfo pageInfo = new PageInfo(page, pageSize);

            ApiResponse<Comment> comment = await _commentService.GetCommentByIdAsync(commentId);

            if (!comment.IsSuccess)
            {
                return NotFound();
            }

            ApiResponse<CommentHistory> result = await _commentHistoryService.GetCommentHistoryByIdAsync(commentId, pageInfo);

            if (!result.IsSuccess)
            {
                return NotFound();
            }

            CommentHistoryViewModel viewModel = new CommentHistoryViewModel
            {
                Comment = comment.Content[0],
                CommentHistory = result.Content,
                PageInfo = result.PageInfo
            };

            return View(viewModel);
        }
    }
}
