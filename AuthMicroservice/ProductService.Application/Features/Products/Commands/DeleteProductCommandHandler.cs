using MediatR;
using ProductService.Persistence.Contexts;

namespace ProductService.Application.Features.Products.Commands;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly ProductDbContext _context;

    public DeleteProductCommandHandler(ProductDbContext context) => _context = context;

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FindAsync(request.Id);
        if (product == null) return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}