using MediatR;
using ProductService.Core.Entities;

namespace ProductService.Application.Features.Products.Queries;

public class GetProductByIdQuery : IRequest<Product>
{
    public int Id { get; set; }
}