using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OrderService.API.Contexts;
using System.Text;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// 1. Veritabanı Bağlantısı
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

// 2. Authentication (Kimlik Doğrulama) Ayarları - JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "http://localhost",
            ValidAudience = "http://localhost",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("CokGizliVeUzunBirGuvenlikAnahtari123!"))
        };
    });

// 3. Authorization (Yetkilendirme) ve Policy Ayarları
builder.Services.AddAuthorization(options =>
{
    // Role-Based Yetkilendirme (Örn: Sadece Admin)
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));

    // Policy-Based Yetkilendirme (Örn: IT Departmanı)
    options.AddPolicy("ITDepartmentOnly", policy => policy.RequireClaim("Department", "IT"));
});

// 4. RabbitMQ ve MassTransit Konfigürasyonu (Event-Driven Mimari)
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h => {
            h.Username("guest");
            h.Password("guest");
        });
    });
});

// Swagger (Test Ekranı) Ayarları
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Tarayıcıda Swagger'ı Göster
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// KİMLİK DOĞRULAMA VE YETKİLENDİRME SIRALAMASI ÇOK ÖNEMLİ
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();