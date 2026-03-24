using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.Core.Application.Products.Commands;
using ProductService.Core.Application.Products.Queries;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _mediator.Send(new GetAllProductsQuery());
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(id);
        }
    }
}