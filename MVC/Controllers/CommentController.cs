using Microsoft.AspNetCore.Mvc;
using MVC.Helpers;
using MVC.Models.ViewModels;
using MVC.Models;
using MVC.Services;
using Common.Dto;
using Common.Dto.Comment;

namespace MVC.Controllers
{
    public class CommentController : Controller
    {
        private readonly CommentServiceOld _commentService;
        public CommentController(CommentServiceOld commentService)
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

            ApiResponseOld<Comment> response = await _commentService.CreateCommentAsync(viewModel, bearerToken);

            if (!response.IsSuccess)
            {
                return RedirectToAction("CreateComment", new { postId = viewModel.PostId });
            }

            return RedirectToAction("GetPostById", "Post", new {postId = viewModel.PostId });
        }

        [HttpGet("Comment/Update")]
        public async Task<IActionResult> Update(int commentId = 0)
        {
            if (commentId <= 0)
            {
                return BadRequest();
            }

            ApiResponseOld<Comment> response = await _commentService.GetCommentByIdAsync(commentId);

            UpdateCommentViewModel updateCommentViewModel = new UpdateCommentViewModel
            {
                Comment = response.Content[0],
                CommentId = commentId
            };

            return View(updateCommentViewModel);
        }


        [HttpPost("Comment/Update")]
        public async Task<IActionResult> Update(int commentId, UpdateCommentDto updateCommentDto)
        {
            if (commentId <= 0 || updateCommentDto == null)
            {
                return BadRequest();
            }

            string? bearerToken = HttpContext.Session.GetJson<string>("Bearer");
            if (string.IsNullOrEmpty(bearerToken))
            {
                return RedirectToAction("Index", "Login");
            }

            ApiResponseOld<Comment> response = await _commentService.UpdateCommentByIdAsync(commentId, updateCommentDto, bearerToken);
            if (!response.IsSuccess)
            {
                return BadRequest();
            }

            return RedirectToAction("GetPostById", "Post", new { postId = response.Content[0].PostId } );
        }

        [HttpPost("Comment/Delete")]
        public async Task<IActionResult> DeleteComment(int commentId, int postId)
        {
            if (commentId <= 0)
            {
                return BadRequest();
            }

            string? bearerToken = HttpContext.Session.GetJson<string>("Bearer");
            if (string.IsNullOrEmpty(bearerToken))
            {
                return RedirectToAction("Index", "Login");
            }

            ApiResponseOld<bool> response = await _commentService.DeleteCommentAsync(commentId, bearerToken);
            if (!response.IsSuccess)
            {
                return BadRequest();
            }

            return RedirectToAction("GetPostById", "Post", new { postId = postId });
        }
    }
}
