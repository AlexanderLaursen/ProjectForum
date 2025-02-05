using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Models.ViewModels;
using MVC.Services;
using System.Diagnostics;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CategoryService _categoryService;

        public HomeController(ILogger<HomeController> logger, CategoryService categoryService)
        {
            _logger = logger;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.IsLoggedIn = HttpContext.Session.GetString("Bearer") != null;

            ApiResponse<Category> apiResponse = await _categoryService.GetCategoriesAsync();

            HomeViewModel viewModel = new HomeViewModel
            {
                Categories = apiResponse.Content
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
