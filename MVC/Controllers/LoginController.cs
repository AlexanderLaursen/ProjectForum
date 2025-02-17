using Common.Models;
using Microsoft.AspNetCore.Mvc;
using MVC.Helpers;
using MVC.Models;
using MVC.Services;

namespace MVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly AuthService _authService;

        public LoginController(AuthService loginService)
        {
            _authService = loginService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Index(LoginData loginData)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Result<BearerToken> result = await _authService.LoginAsync(loginData);

            if (!result.IsSuccess || result.Value?.AccessToken == null)
            {
                return View();
            }

            HttpContext.Session.SetJson("Bearer", result.Value.AccessToken);

            ApiResponseOld<string> userIdResponse = await _authService.GetUserIdByUsernameAsync(loginData.Email);

            if (userIdResponse.IsSuccess)
            {
                HttpContext.Session.SetJson("Username", loginData.Email);
                HttpContext.Session.SetJson("UserId", userIdResponse.Content[0]);
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpGet("/logut")]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("Bearer");
            return RedirectToAction("Index", "Home");
        }
    }
}
