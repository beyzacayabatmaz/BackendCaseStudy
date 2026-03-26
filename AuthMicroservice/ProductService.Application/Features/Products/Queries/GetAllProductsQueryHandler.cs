using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Core.Entities;
using ProductService.Persistence.Contexts;

namespace ProductService.Application.Features.Products.Queries
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<Product>>
    {
        private readonly ProductDbContext _context;

        public GetAllProductsQueryHandler(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            // Eğer Products listesi hala kırmızıysa, ProductDbContext içinde 
            // DbSet<Product> Products { get; set; } satırı eksik olabilir.
            return await _context.Products.ToListAsync(cancellationToken);
        }
    }
}