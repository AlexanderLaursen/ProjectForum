﻿using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Models.ViewModels;
using MVC.Services;

namespace MVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryService _categoryService;
        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("Category/{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            if (id <= 0)
            {
                return View("Home");
            }

            ApiResponse<Category> apiResponse = await _categoryService.GetCategoryByIdAsync(id);

            CategoryViewModel viewModel = new CategoryViewModel
            {
                Category = apiResponse.Content[0]
            };

            return View("Categories", viewModel);
        }

    }
}
