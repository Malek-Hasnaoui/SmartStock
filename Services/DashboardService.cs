
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

            dashboard.TotalProducts = products.Count;

            dashboard.TotalCategories =
                await _context.Categories.CountAsync();

            dashboard.TotalStockValue = products.Sum(
                p => p.Price * p.Quantity
            );

            dashboard.LowStockProducts = products
                .Where(p => p.Quantity <= p.MinimumStock)
                .ToList();

            dashboard.LowStockCount =
                dashboard.LowStockProducts.Count;

            return dashboard;
        }
    }
}
