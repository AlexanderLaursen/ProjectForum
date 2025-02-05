using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Models.ViewModels;
using MVC.Services;

namespace MVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginService _loginService;

        public LoginController (LoginService loginService)
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
                HttpContext.Session.SetString("Bearer", response.Token.AccessToken);
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}
