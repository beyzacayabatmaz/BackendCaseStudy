using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using ProductService.Infrastructure.Persistence;
using ProductService.Core.Domain;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace ProductService.Core.Application.Products.Commands
{
    
    public record CreateProductCommand(string Name, decimal Price, int Stock) : IRequest<int>;

    
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _cache;

        public CreateProductCommandHandler(ApplicationDbContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            
            var product = new Product
            {
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);

            
            await _cache.RemoveAsync("productList", cancellationToken);

            
            try
            {
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using var connection = await factory.CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();

                await channel.QueueDeclareAsync(queue: "product_created_queue",
                                                durable: false,
                                                exclusive: false,
                                                autoDelete: false,
                                                arguments: null);

                var message = JsonSerializer.Serialize(product);
                var body = Encoding.UTF8.GetBytes(message);

                await channel.BasicPublishAsync(exchange: string.Empty,
                                                routingKey: "product_created_queue",
                                                body: body);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"RabbitMQ Hatası: {ex.Message}");
            }

            return product.Id;
        }
    }
}