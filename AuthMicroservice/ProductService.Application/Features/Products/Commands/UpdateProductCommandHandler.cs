using MediatR;
using ProductService.Persistence.Contexts;

namespace ProductService.Application.Features.Products.Commands;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
{
    private readonly ProductDbContext _context;

    public UpdateProductCommandHandler(ProductDbContext context) => _context = context;

    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FindAsync(request.Id);
        if (product == null) return false;

        product.Name = request.Name;
        product.Price = request.Price;
        product.Stock = request.Stock;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}