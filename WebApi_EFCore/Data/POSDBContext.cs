using Microsoft.EntityFrameworkCore;
using POSSystem2.Models;
using POSSystem2.Models;

namespace POSSystem2.Data
{
    public class PosDbContext : DbContext
    {
        public PosDbContext(DbContextOptions<PosDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasKey(p => p.Sku);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .HasKey(o => o.Id);

            modelBuilder.Entity<CartItem>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<CartItem>()
                .Property(c => c.UnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.Items)
                .WithOne(c => c.Order)
                .HasForeignKey(c => c.OrderId);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.CartItems)
                .WithOne(c => c.Product)
                .HasForeignKey(c => c.Sku);

            modelBuilder.Entity<Product>().HasData(
                new POSSystem2.Models.Product
                {
                    Sku = "P001",
                    Name = "Blue T-Shirt",
                    Price = 19.99m,
                    Category = "Apparel",
                    Stock = 100
                },
                new POSSystem2.Models.Product
                {
                    Sku = "P002",
                    Name = "Coffee Mug",
                    Price = 7.5m,
                    Category = "Home",
                    Stock = 200
                },
                new POSSystem2.Models.Product
                {
                    Sku = "P003",
                    Name = "Notebook",
                    Price = 3.25m,
                    Category = "Office",
                    Stock = 500
                }
            );
        }
    }
}