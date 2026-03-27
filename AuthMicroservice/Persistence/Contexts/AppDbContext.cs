using Microsoft.EntityFrameworkCore;

namespace Persistence.Contexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    // Eğer Auth için tabloların varsa onlar buradaydı, şimdilik böyle kalması hatayı çözer
}