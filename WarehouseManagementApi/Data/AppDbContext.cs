using Microsoft.EntityFrameworkCore;
using WarehouseManagementApi.Models;
using WarehouseManagementApi.Models.User;

namespace WarehouseManagementApi.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
