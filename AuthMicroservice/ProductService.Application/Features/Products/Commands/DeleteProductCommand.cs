using MediatR;

namespace ProductService.Application.Features.Products.Commands;

public class DeleteProductCommand : IRequest<bool> 
{
    public int Id { get; set; }
}