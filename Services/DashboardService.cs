
using Microsoft.EntityFrameworkCore;
using SmartStock.Data;
using SmartStock.Models;

namespace SmartStock.Services
{
    public class DashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardViewModel> GetDashboardDataAsync()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .ToListAsync();

            var dashboard = new DashboardViewModel();

            // Total number of products
            dashboard.TotalProducts = products.Count;

            // Total number of categories
            dashboard.TotalCategories =
                await _context.Categories.CountAsync();

            // Total available quantity
            dashboard.TotalStock = products.Sum(p => p.Quantity);

            // Total inventory value = Price * Quantity
            dashboard.TotalStockValue = products.Sum(
                p => p.Price * p.Quantity
            );

            // Products requiring restocking
            dashboard.LowStockProducts = products
                .Where(p => p.Quantity <= p.MinimumStock)
                .ToList();

            dashboard.LowStockCount =
                dashboard.LowStockProducts.Count;

            return dashboard;
        }
    }
}
