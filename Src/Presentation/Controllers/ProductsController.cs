using Application.DTOs;
using Application.Features.Products.Commands.CreateProduct;
using Application.Features.Products.Commands.UpdateProduct;
using Application.Features.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Domain.Common;

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
            var result = await _mediator.Send(command);
            return result.Match(
                onSuccess: id => CreatedAtAction(nameof(GetProductById), new { id }, id),
                onFailure: error => error.StatusCode.HasValue
                                    ? StatusCode(error.StatusCode.Value, error)
                                    : BadRequest(error));
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

        // PUT: api/products/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch");

            var result = await _mediator.Send(command);

            if (result.IsSuccess)
                return NoContent(); // 204 No Content

            if (result.Error?.StatusCode.HasValue == true)
                return StatusCode(result.Error.StatusCode.Value, result.Error);

            return BadRequest(result.Error);
        }
    }
}