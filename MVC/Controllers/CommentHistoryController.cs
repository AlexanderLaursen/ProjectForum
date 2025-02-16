using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Models.ViewModels;
using Common.Models;
using MVC.Services.Interfaces;
using Common.Enums;
using System.Diagnostics;

namespace MVC.Controllers
{
    public class CommentHistoryController : Controller
    {
        private readonly ICommentHistoryApiService _commentHistoryService;
        private readonly ILogger<CommentHistoryController> _logger;
        public CommentHistoryController(ICommentHistoryApiService commentHistoryService, ILogger<CommentHistoryController> logger)
        {
            _commentHistoryService = commentHistoryService;
            _logger = logger;
        }   

        [HttpGet("/comment-history/{commentId}")]
        public async Task<IActionResult> GetCommentHistory(int commentId, int page, int pageSize)
        {
            if (commentId <= 0)
            {
                return BadRequest();
            }

            PageInfo pageInfo = new PageInfo(page, pageSize);
            Result<CommentHistories> result = await _commentHistoryService.GetCommentHistoryAsync(commentId, pageInfo);

            if (result.IsSuccess && result.Value != null)
            {
                CommentHistoryViewModel viewModel = new CommentHistoryViewModel
                {
                    Comment = result.Value.Comment,
                    CommentHistory = result.Value.CommentHistory,
                    PageInfo = result.Value.PageInfo
                };

                return View(viewModel);
            }

            _logger.LogError(result.ErrorMessage);
            return View("Error", new ErrorViewModel());
        }
    }
}
