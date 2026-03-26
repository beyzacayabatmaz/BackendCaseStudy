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
    // IRequestHandler: "Ben GetAllProductsQuery isteğini alır, geriye List<Product> dönerim" diyoruz.
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<Product>>
    {
        private readonly ProductDbContext _context;

        public GetAllProductsQueryHandler(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            // Veritabanına gidip tüm ürünleri asenkron olarak listeliyoruz.
            return await _context.Products.ToListAsync(cancellationToken);
        }
    }
}