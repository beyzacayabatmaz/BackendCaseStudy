using MediatR;
using ProductService.Persistence.Contexts;
using ProductService.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ProductService.Application.Features.Products.Queries;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product>
{
    private readonly ProductDbContext _context;

    public GetProductByIdQueryHandler(ProductDbContext context)
    {
        _context = context;
    }

    public async Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        // Veritabanında ID'ye göre arama yapıyoruz
        return await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
    }
}