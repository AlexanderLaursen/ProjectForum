using Microsoft.AspNetCore.Mvc;
using MVC.Helpers;
using MVC.Models.ViewModels;
using MVC.Models;
using MVC.Services;

namespace MVC.Controllers
{
    public class CommentController : Controller
    {
        private readonly CommentService _commentService;
        public CommentController(CommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("Comment/CreateComment")]
        public IActionResult CreateComment(int postId = 0)
        {
            CreateCommentViewModel createCommentViewModel = new CreateCommentViewModel
            {
                PostId = postId
            };
            return View(createCommentViewModel);
        }

        [HttpPost("Comment/CreateComment")]
        public async Task<IActionResult> CreateComment(CreateCommentViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("CreateComment", new { postId = viewModel.PostId });
            }

            string? bearerToken = HttpContext.Session.GetJson<string>("Bearer");

            if (string.IsNullOrEmpty(bearerToken))
            {
                return RedirectToAction("Index", "Login");
            }

            ApiResponse<Comment> response = await _commentService.CreateCommentAsync(viewModel, bearerToken);

            if (!response.IsSuccess)
            {
                return RedirectToAction("CreateComment", new { postId = viewModel.PostId });
            }

            return RedirectToAction("GetPostById", "Post", new {postId = viewModel.PostId });
        }

    }
}
