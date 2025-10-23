using Microsoft.EntityFrameworkCore;
using WarehouseManagementApi.Models;

namespace WarehouseManagementApi.Data
{
    public class ProductContext: DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options): base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}
