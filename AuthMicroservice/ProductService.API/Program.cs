using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ProductService.Persistence.Contexts;
using System.Reflection;
using ProductService.Application.Features.Products.Queries; // Handler'ın olduğu yer

var builder = WebApplication.CreateBuilder(args);

// 1. Controller ve API Desteği
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// 2. Veritabanı Bağlantısı (appsettings.json'daki DefaultConnection'ı kullanır)
builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. MediatR Kaydı (Kritik Nokta)
// Handler'ın olduğu sınıfı açıkça göstererek MediatR'ın tüm Application katmanını taramasını sağlıyoruz.
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(GetAllProductsQueryHandler).Assembly);
});

var app = builder.Build();

// 4. Middleware (Ara Yazılım) Yapılandırması
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// SSL/HTTPS hatalarını aşmak için burayı kapalı tutuyoruz (önceki adımda konuştuğumuz gibi)
// app.UseHttpsRedirection(); 

app.UseAuthorization();
app.MapControllers();

app.Run();