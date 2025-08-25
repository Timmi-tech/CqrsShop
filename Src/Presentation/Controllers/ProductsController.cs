using Application.Common;
using Application.DTOs;
using Application.Features.Products.Commands.CreateProduct;
using Application.Features.Products.Commands.DeleteProduct;
using Application.Features.Products.Commands.UpdateProduct;
using Application.Features.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Domain.Common;
using Microsoft.AspNetCore.Authorization;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/Products")]
    [Authorize]
    public class ProductsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        // POST: api/products
        [HttpPost]
        [Authorize(Roles = "Admin")]
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
        public async Task<IActionResult> GetAllProducts([FromQuery] PaginationParameters? pagination)
        {
            if (pagination != null)
            {
                PagedResult<ProductDto> pagedProducts = await _mediator.Send(new GetProductsPagedQuery(pagination));
                return Ok(pagedProducts);
            }

            IEnumerable<ProductDto> products = await _mediator.Send(new GetAllProductsQuery());
            return Ok(products);
        }

        // PUT: api/products/{id}
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
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

        // DELETE: api/products/{id}
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var result = await _mediator.Send(new DeleteProductCommand(id));

            if (result.IsSuccess)
                return NoContent();

            if (result.Error?.StatusCode.HasValue == true)
                return StatusCode(result.Error.StatusCode.Value, result.Error);

            return BadRequest(result.Error);
        }
    }
}