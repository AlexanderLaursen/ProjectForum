using Microsoft.AspNetCore.Mvc;
using MVC.Helpers;
using MVC.Models;
using MVC.Models.ViewModels;
using MVC.Services;

namespace MVC.Controllers
{
    public class PostController : Controller
    {
        private readonly PostService _postService;
        private readonly CategoryService _categoryService;
        public PostController(PostService postService, CategoryService categoryService)
        {
            _postService = postService;
            _categoryService = categoryService;
        }

        [HttpGet("Category/{categoryId}/posts")]
        public async Task<IActionResult> GetPostsByCategoryId(int categoryId, int page, int pageSize)
        {
            if (categoryId <= 0)
            {
                return BadRequest("Invalid category id.");
            }
            PageInfo pageInfo = new PageInfo(page, pageSize);
            ApiResponse<Post> response = await _postService.GetPostsByCategoryIdAsync(categoryId, pageInfo);
            ApiResponse<Category> categoryResponse = await _categoryService.GetCategoryByIdAsync(categoryId);

            if (!response.IsSuccess)
            {
                return NotFound();
            }

            PostsViewModel viewModel = new PostsViewModel
            {
                Category = categoryResponse.Content[0],
                Posts = response.Content,
                PageInfo = response.PageInfo
            };

            return View("Posts", viewModel);
        }

        [HttpGet("Post/{postId}")]
        public async Task<IActionResult> GetPostById(int postId)
        {
            if (postId <= 0)
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

        [HttpGet("Post/CreatePost")]
        public IActionResult CreatePost(int categoryId = 0)
        {
            CreatePostViewModel createPostViewModel = new CreatePostViewModel
            {
                CategoryId = categoryId
            };
            return View(createPostViewModel);
        }

        [HttpPost("Post/CreatePost")]
        public async Task<IActionResult> CreatePost(CreatePostViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("CreatePost", new { categoryId = viewModel.CategoryId });
            }

            string? bearerToken = HttpContext.Session.GetJson<string>("Bearer");

            if (string.IsNullOrEmpty(bearerToken))
            {
                return RedirectToAction("Index", "Login");
            }

            ApiResponse<Post> response = await _postService.CreatePostAsync(viewModel, bearerToken);

            if (!response.IsSuccess)
            {
                return RedirectToAction("CreatePost", new { categoryId = viewModel.CategoryId });
            }

            return RedirectToAction("GetPostById", new { postId = response.Content[0].Id });
        }
    }
}
