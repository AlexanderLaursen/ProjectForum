using Common.Models;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Models.ViewModels;
using MVC.Services.Interfaces;
using System.Diagnostics;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryApiService _categoryService;

        public HomeController(ILogger<HomeController> logger, ICategoryApiService categoryService)
        {
            _logger = logger;
            _categoryService = categoryService;
        }

        [HttpGet()]
        public async Task<IActionResult> Index()
        {
            Result<List<Category>> categories = await _categoryService.GetCategoriesAsync();

            if (categories.IsSuccess && categories.Value != null)
            {
                HomeViewModel viewModel = new HomeViewModel
                {
                    Categories = categories.Value
                };

                return View(viewModel);
            }
            else
            {
                _logger.LogError(categories.ErrorMessage);
                return View("Error");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
