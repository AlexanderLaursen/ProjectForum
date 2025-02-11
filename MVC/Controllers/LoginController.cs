using Microsoft.AspNetCore.Mvc;
using MVC.Helpers;
using MVC.Models;
using MVC.Models.ViewModels;
using MVC.Services;

namespace MVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly AuthService _loginService;

        public LoginController(AuthService loginService)
        {
            _loginService = loginService;
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

            LoginResponse response = await _loginService.LoginAsync(loginData);

            if (!response.IsSuccess)
            {
                return View();
            }

            HttpContext.Session.SetJson("Bearer", response.Token.AccessToken);

            ApiResponse<string> userIdResponse = await _loginService.GetUserIdByUsernameAsync(loginData.Email);

            if (userIdResponse.IsSuccess)
            {
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
