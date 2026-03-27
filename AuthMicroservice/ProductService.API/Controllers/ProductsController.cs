using Microsoft.AspNetCore.Mvc;
using MediatR;
using ProductService.Application.Features.Products.Queries;

namespace ProductService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // Test Metodu (Ping)
    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok("Product API Ayakta!");
    }

    // Gerçek Veri Metodu
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllProductsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}