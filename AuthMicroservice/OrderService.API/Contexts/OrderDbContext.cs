using Microsoft.EntityFrameworkCore;
using OrderService.API.Entities;

namespace OrderService.API.Contexts;

public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }
    public DbSet<Order> Orders { get; set; } = null!;
}