using MediatR;
using ProductService.Core.Entities;
using ProductService.Persistence.Contexts;

namespace ProductService.Application.Features.Products.Commands;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
{
    private readonly ProductDbContext _context;

    public CreateProductCommandHandler(ProductDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
          
            Name = request.Name,
            Price = request.Price,
            Stock = request.Stock,
            CreatedDate = DateTime.Now 
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync(cancellationToken);

        return product.Id; 
    }
}