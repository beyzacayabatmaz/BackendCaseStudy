using MediatR;

namespace ProductService.Application.Features.Products.Commands;

public class CreateProductCommand : IRequest<int> // Geriye int dönecek
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
}