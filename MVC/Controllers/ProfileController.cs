using Microsoft.AspNetCore.Mvc;
using MVC.Helpers;
using MVC.Models;
using MVC.Models.ViewModels;
using MVC.Services;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MVC.Controllers
{
    public class ProfileController : Controller
    {
        private readonly PostService _postService;
        private readonly CommentService _commentService;
        private readonly UserService _userService;

        public ProfileController (PostService postService, CommentService commentService, UserService userService)
        {
            _postService = postService;
            _commentService = commentService;
            _userService = userService;
        }

        [HttpGet("/profile")]
        public async Task<IActionResult> OwnProfile()
        {
            string? username = HttpContext.Session.GetJson<string>("Username");

            if (string.IsNullOrWhiteSpace(username))
            {
                return RedirectToAction("Login", "Account");
            }

            return RedirectToAction("Index", new { username });
        }

        [HttpGet("/profile/{username}")]
        public async Task<IActionResult> Index(string username, int postPage = 0, int postSize = 0, int commentPage = 0, int commentSize = 0)
        {
            ApiResponse<AppUser> user = await _userService.GetUserByUsernameAsync(username);


            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            PageInfo postPageInfo = new(postPage, postSize);
            ApiResponse<Post> posts = await _postService.GetPostsByUserIdAsync(username, postPageInfo);

            PageInfo commentPageInfo = new(commentPage, commentSize);
            ApiResponse<Comment> comments = await _commentService.GetCommentsByUserIdAsync(username, commentPageInfo);

            ProfileViewModel viewModel = new()
            {
                User = user.Content[0],
                Posts = posts.Content,
                Comments = comments.Content,
                PostPageInfo = posts.PageInfo,
                CommentPageInfo = comments.PageInfo
            };

            return View(viewModel);
        }

        [HttpPost("/profile/upload")]
        public async Task<IActionResult> UploadProfilePicture(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("", "No file uploaded.");
                return RedirectToAction("OwnProfile");
            }

            string? username = HttpContext.Session.GetJson<string>("Username");
            string? bearerToken = HttpContext.Session.GetJson<string>("Bearer");

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(bearerToken))
            {
                return RedirectToAction("Login", "Account");
            }


            var response = await _userService.UploadPictureAsync(file, username, bearerToken);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("OwnProfile");
            }

            ModelState.AddModelError("", "Failed to upload profile picture.");
            return RedirectToAction("OwnProfile");
        }
    }
}
