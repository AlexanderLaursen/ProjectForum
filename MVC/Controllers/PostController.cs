using Common.Enums;
using Common.Models;
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

        [HttpGet("/Category/{categoryId}/posts")]
        public async Task<IActionResult> GetPostsByCategoryId(PostsViewModel oldViewModel, int categoryId, int page, int pageSize, SortDirection sortDirection = 0, SortBy sortBy = 0)
        {
            if (categoryId <= 0)
            {
                return BadRequest("Invalid category id.");
            }
            PageInfo pageInfo = new PageInfo(page, pageSize);
            ApiResponseOld<Post> response = await _postService.GetPostsByCategoryIdAsync(categoryId, pageInfo, sortDirection, sortBy);
            ApiResponseOld<Category> categoryResponse = await _categoryService.GetCategoryByIdAsync(categoryId);

            if (!response.IsSuccess)
            {
                return NotFound();
            }

            PostsViewModel viewModel = new PostsViewModel
            {
                Category = categoryResponse.Content[0],
                Posts = response.Content,
                PageInfo = response.PageInfo,
                SortBy = sortBy,
                SortDirection = sortDirection
            };

            return View("Posts", viewModel);
        }

        [HttpGet("/Post/{postId}")]
        public async Task<IActionResult> GetPostById(int postId, int page, int pageSize)
        {
            if (postId <= 0)
            {
                return BadRequest();
            }

            PageInfo pageInfo = new PageInfo(page, pageSize);
            ApiResponseOld<Post> response = await _postService.GetPostByIdAsync(postId, pageInfo);

            if (!response.IsSuccess)
            {
                return NotFound();
            }

            string userId = HttpContext.Session.GetJson<string>("UserId")!;

            PostViewModel viewModel = new PostViewModel
            {
                UserId = userId!,
                Post = response.Content[0],
                Comments = response.Content[0].Comments,
                PageInfo = response.PageInfo
            };

            return View("Post", viewModel);
        }

        [HttpGet("/Post/CreatePost")]
        public IActionResult CreatePost(int categoryId = 0)
        {
            CreatePostViewModel createPostViewModel = new CreatePostViewModel
            {
                CategoryId = categoryId
            };
            return View(createPostViewModel);
        }

        [HttpPost("/Post/CreatePost")]
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

            ApiResponseOld<Post> response = await _postService.CreatePostAsync(viewModel, bearerToken);

            if (!response.IsSuccess)
            {
                return RedirectToAction("CreatePost", new { categoryId = viewModel.CategoryId });
            }

            return RedirectToAction("GetPostById", new { postId = response.Content[0].Id });
        }

        [HttpGet("/Post/Update")]
        public async Task<IActionResult> Update(int postId, UpdatePostViewModel updatePostViewModel)
        {
            if (postId <= 0)
            {
                return BadRequest();
            }

            ApiResponseOld<Post> response = await _postService.GetPostByIdAsync(postId, new PageInfo());

            UpdatePostViewModel viewModel = new()
            {
                Post = response.Content[0],
                CategoryId = response.Content[0].CategoryId,
                PostId = postId,
            };

            return View(viewModel);
        }

        [HttpPost("/Post/Update")]
        public async Task<IActionResult> Update(UpdatePostViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Update", new { postId = viewModel.PostId });
            }

            string? bearerToken = HttpContext.Session.GetJson<string>("Bearer");
            if (string.IsNullOrEmpty(bearerToken))
            {
                return RedirectToAction("Index", "Login");
            }

            ApiResponseOld<Post> response = await _postService.UpdatePostAsync(viewModel, bearerToken);
            if (!response.IsSuccess)
            {
                return RedirectToAction("Update", new { postId = viewModel.PostId });
            }

            return RedirectToAction("GetPostById", new { postId = viewModel.PostId });
        }

        [HttpPost("/Post/Delete")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            if (postId <= 0)
            {
                return BadRequest();
            }

            string? bearerToken = HttpContext.Session.GetJson<string>("Bearer");
            if (string.IsNullOrEmpty(bearerToken))
            {
                return RedirectToAction("Index", "Login");
            }

            ApiResponseOld<bool> response = await _postService.DeletePostAsync(postId, bearerToken);
            if (!response.IsSuccess)
            {
                return BadRequest();
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
