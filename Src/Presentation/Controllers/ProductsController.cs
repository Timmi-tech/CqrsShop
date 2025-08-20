using Application.DTOs;
using Application.Features.Products.Commands.CreateProduct;
using Application.Features.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/Products")]
    public class ProductsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        // POST: api/products
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
        {
            Guid productId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetProductById), new { id = productId }, productId);
        }

        // GET: api/products/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            IEnumerable<ProductDto> products = await _mediator.Send(new GetProductsByIdQuery([id]));
            return Ok(products);
        }
        // GET: api/products
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            IEnumerable<ProductDto> products = await _mediator.Send(new GetAllProductsQuery());
            return Ok(products);
        }

    }
}