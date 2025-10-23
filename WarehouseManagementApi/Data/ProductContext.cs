using Microsoft.EntityFrameworkCore;
using WarehouseManagementApi.Models;

namespace WarehouseManagementApi.Data
{
    public class ProductContext(DbContextOptions<ProductContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }
    }
}
