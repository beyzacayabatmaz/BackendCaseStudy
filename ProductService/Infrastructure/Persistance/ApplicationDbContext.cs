using Microsoft.EntityFrameworkCore;
using ProductService.Core.Domain;

namespace ProductService.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Akıllı Saat", Price = 3500, Stock = 50 },
                new Product { Id = 2, Name = "Kablosuz Kulaklık", Price = 1200, Stock = 100 },
                new Product { Id = 3, Name = "Oyuncu Mouse", Price = 850, Stock = 30 }
            );
        }
    }
}