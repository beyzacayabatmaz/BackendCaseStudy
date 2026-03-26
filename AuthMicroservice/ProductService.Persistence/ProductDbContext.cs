using Microsoft.EntityFrameworkCore; // Bu satır olmazsa DbContext hata verir
using ProductService.Core.Entities;
using System.Reflection.Emit;

namespace ProductService.Persistence.Contexts
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().Property(p => p.Price).HasColumnType("decimal(18,2)");
            base.OnModelCreating(modelBuilder);
        }
    }
}