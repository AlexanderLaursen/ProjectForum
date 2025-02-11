using Microsoft.AspNetCore.Mvc;
using MVC.Helpers;
using MVC.Models;
using MVC.Services;

namespace MVC.Controllers
{
    public class ProfileController : Controller
    {
        private readonly PostService _postService;
        private readonly CommentService _commentService;
        private readonly UserService _userService;


        [HttpGet("/profile")]
        public async IActionResult Index(int postPage = 0, int postSize = 0, int commentPage = 0, int commentSize = 0)
        {
            string username = HttpContext.Session.GetJson<string>("Username");

            PageInfo postPageInfo = new(postPage, postSize);
            ApiResponse<Post> posts = await _postService.GetPostsByUserIdAsync(username, postPageInfo);

            PageInfo commentPageInfo = new(commentPage, commentSize);
            ApiResponse<Comment> comments = await _commentService.GetCommentsByUserIdAsync(username, commentPageInfo);

            return View();
        }
    }
}
