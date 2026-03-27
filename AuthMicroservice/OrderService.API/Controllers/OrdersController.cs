using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.API.Contexts;
using OrderService.API.Entities;
using OrderService.API.Events; // Event sınıfımız için eklendi
using MassTransit; // RabbitMQ arayüzü için eklendi

namespace OrderService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly OrderDbContext _context;
    private readonly IPublishEndpoint _publishEndpoint; // MassTransit mesaj fırlatıcısı

    // Constructor'a IPublishEndpoint'i dahil ettik
    public OrdersController(OrderDbContext context, IPublishEndpoint publishEndpoint)
    {
        _context = context;
        _publishEndpoint = publishEndpoint;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _context.Orders.ToListAsync();
        return Ok(orders);
    }

    [HttpPost]
    [Authorize(Policy = "ITDepartmentOnly")]
    public async Task<IActionResult> CreateOrder(Order order)
    {
        // 1. Siparişi kendi veritabanımıza kaydediyoruz
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        // TASARIM KARARI: Event-Driven Mimari ve SAGA Pattern
        // Sipariş oluşturulduğunda diğer servisleri (örn: Stok/Ürün) doğrudan bekletmemek 
        // ve sistemi asenkron/ölçeklenebilir hale getirmek için MassTransit aracılığıyla 
        // RabbitMQ kuyruğuna OrderCreatedEvent fırlatıyoruz. 
        // Bu sayede dağıtık transaction (SAGA) sürecini başlatmış oluyoruz.

        // 2. RabbitMQ Kuyruğuna Mesaj (Event) Fırlatıyoruz
        var orderCreatedEvent = new OrderCreatedEvent
        {
            OrderId = order.Id,
            ProductId = order.ProductId,
            Quantity = order.Quantity
        };
        await _publishEndpoint.Publish(orderCreatedEvent); // Mesaj kuyruğa gitti!

        return Ok("Sipariş başarıyla oluşturuldu ve RabbitMQ kuyruğuna iletildi!");
    }
}