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

        [HttpPost]
        public async Task<IActionResult> Index(LoginData loginData)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            LoginResponse response = await _loginService.LoginAsync(loginData);

            if (response.IsSuccess)
            {
                HttpContext.Session.SetJson("Bearer", response.Token.AccessToken);
                //HttpContext.Session.SetString("Bearer", response.Token.AccessToken);
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("Bearer");
            return RedirectToAction("Index", "Home");
        }


    }
}
