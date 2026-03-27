using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Core.Entities;
using ProductService.Persistence.Contexts;

namespace ProductService.Application.Features.Products.Queries;

public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<Product>>
{
    private readonly ProductDbContext _context;

    public GetAllProductsQueryHandler(ProductDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
       
        return await _context.Products.ToListAsync(cancellationToken);

    }
}