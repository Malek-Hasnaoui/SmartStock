
using SmartStock.Models;

namespace SmartStock.Models
{
    public class DashboardViewModel
    {
        public int TotalProducts { get; set; }

        public int TotalCategories { get; set; }

        public int TotalStock { get; set; }

        public decimal TotalStockValue { get; set; }

        public int LowStockCount { get; set; }

        public List<Product> LowStockProducts { get; set; }
            = new List<Product>();
    }
}
