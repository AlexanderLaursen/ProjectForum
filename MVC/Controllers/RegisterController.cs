using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Services;

namespace MVC.Controllers
{
    public class RegisterController : Controller
    {
        private readonly AuthService _authService;
        public RegisterController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("/register")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("/register")]
        public async Task<IActionResult> Register(RegisterData registerData)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            if (registerData.Password != registerData.ConfirmPassword)
            {
                return View("Index");
            }

            LoginData loginData = new LoginData
            {
                Email = registerData.Email,
                Password = registerData.Password
            };

            ApiResponse<object> response = await _authService.RegisterAsync(loginData);

            if (response.IsSuccess)
            {
                return RedirectToAction("Index", "Login");
            }
            return View("Index");
        }
    }
}
