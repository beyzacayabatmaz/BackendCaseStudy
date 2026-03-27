using MediatR;

namespace ProductService.Application.Features.Products.Commands;

public class DeleteProductCommand : IRequest<bool> // Sildi mi silmedi mi? (true/false)
{
    public int Id { get; set; }
}