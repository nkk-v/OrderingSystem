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
        public DbSet<ProductVariant> tblProductVariant { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Product → ProductVariant (Cascade OK)
            modelBuilder.Entity<ProductVariant>()
                .HasOne(pv => pv.Product)
                .WithMany(p => p.Variants)
                .HasForeignKey(pv => pv.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Product → CartItem (Cascade OK)
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // ProductVariant → CartItem (Restrict to avoid multiple cascade path)
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.productVariant)
                .WithMany()
                .HasForeignKey(ci => ci.ProductVariantId)
                .OnDelete(DeleteBehavior.Restrict);

            // Order → OrderItem (Cascade OK)
            modelBuilder.Entity<OrderItem>()
                .HasOne(o => o.Order)
                .WithMany(oi => oi.OrderItems)
                .HasForeignKey(o => o.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // OrderItem → ProductVariant (Restrict to avoid multiple cascade path)
            modelBuilder.Entity<OrderItem>()
                .HasOne(pv => pv.ProductVariant)
                .WithMany()
                .HasForeignKey(pv => pv.ProductVariantId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    
}


