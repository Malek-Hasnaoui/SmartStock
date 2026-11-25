
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SmartStock.Services;
using SmartStock.Models;

namespace SmartStock.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;

        public HomeController(
            ProductService productService,
            CategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllAsync();
            var categories = await _categoryService.GetAllAsync();

            ViewBag.TotalProducts = products.Count;
            ViewBag.TotalCategories = categories.Count;
            ViewBag.TotalStock = products.Sum(p => p.Quantity);

            ViewBag.LowStock = products.Count(
                p => p.Quantity <= p.MinimumStock
            );

            ViewBag.TotalValue = products.Sum(
                p => p.Price * p.Quantity
            );

            return View();
        }
    }
}
