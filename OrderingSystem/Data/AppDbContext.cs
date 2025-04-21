using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OrderingSystem.Models;

namespace OrderingSystem.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { 
        }

        public DbSet<Cart> tblCarts { get; set; }
        public DbSet<Category> tblCategories { get; set; }
        public DbSet<Order> tblOrders { get; set; }
        public DbSet<OrderItem> tblOrderItems { get; set; }
        public DbSet<Payment> tblPayments { get; set; }
        public DbSet<Product> tblProducts { get; set; }
        public DbSet<CartItem> tblCartItems { get; set; }
    }
}
