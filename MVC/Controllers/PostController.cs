using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Models.ViewModels;
using MVC.Services;

namespace MVC.Controllers
{
    public class PostController : Controller
    {
        private readonly PostService _postService;
        public PostController(PostService postService)
        {
            _postService = postService;
        }

        [HttpGet("Category/{categoryId}/posts")]
        public async Task<IActionResult> GetPostsByCategoryId(int categoryId)
        {
            if (categoryId <= 0)
            {
                return BadRequest("Invalid category id.");
            }

            ApiResponse<Post> response = await _postService.GetPostsByCategoryId(categoryId);
            if (!response.IsSuccess)
            {
                return NotFound();
            }

            CategoryViewModel viewModel = new CategoryViewModel
            {
                Posts = response.Content,
                PageInfo = response.PageInfo
            };

            return View("Posts", viewModel);
        }

        [HttpGet("Post/{postId}")]
        public async Task<IActionResult> GetPostById(int postId)
        {
            if (postId  <= 0)
            {
                return BadRequest();
            }

            ApiResponse<Post> response = await _postService.GetPostByIdAsync(postId);

            if (!response.IsSuccess)
            {
                return NotFound();
            }

            PostViewModel viewModel = new PostViewModel
            {
                Post = response.Content[0],
                Comments = response.Content[0].Comments,
                PageInfo = response.PageInfo
            };

            return View("Post", viewModel);
        }
    }
}
