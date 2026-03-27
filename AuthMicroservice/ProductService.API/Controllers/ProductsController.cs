using Microsoft.AspNetCore.Mvc;
using MediatR;
using ProductService.Application.Features.Products.Queries;
using ProductService.Application.Features.Products.Commands;

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

    
    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok("Product API Ayakta!");
    }

    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllProductsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }


    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetProductByIdQuery { Id = id });
        if (result == null)
            return NotFound($"{id} ID'li ürün bulunamadı.");

        return Ok(result);
    }

    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

  
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateProductCommand command)
    {
        var result = await _mediator.Send(command);
        if (result)
            return Ok("Ürün başarıyla güncellendi.");

        return NotFound("Güncellenecek ürün bulunamadı.");
    }

    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteProductCommand { Id = id });
        if (result)
            return Ok($"{id} ID'li ürün başarıyla silindi.");

        return NotFound("Silinecek ürün bulunamadı.");
    }
}