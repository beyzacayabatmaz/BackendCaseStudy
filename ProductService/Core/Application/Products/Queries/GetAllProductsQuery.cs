using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using ProductService.Core.Domain;
using ProductService.Infrastructure.Persistence;

namespace ProductService.Core.Application.Products.Queries
{
    // 1. Dışarıdan gelen İstek (Query)
    public class GetAllProductsQuery : IRequest<List<Product>>
    {
    }

    // 2. İsteği İşleyen Handler (İşin Mutfağı)
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<Product>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _cache; // Redis'i buraya çağırıyoruz

        public GetAllProductsQueryHandler(ApplicationDbContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<List<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
           
            string cacheKey = "productList";

           
            var cachedProducts = await _cache.GetStringAsync(cacheKey, cancellationToken);

            if (!string.IsNullOrEmpty(cachedProducts))
            {

                return JsonSerializer.Deserialize<List<Product>>(cachedProducts) ?? new List<Product>();
            }

            
            var products = await _context.Products.ToListAsync(cancellationToken);

            
            var cacheOptions = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10)); 

            var serializedProducts = JsonSerializer.Serialize(products);
            await _cache.SetStringAsync(cacheKey, serializedProducts, cacheOptions, cancellationToken);

            return products;
        }
    }
}