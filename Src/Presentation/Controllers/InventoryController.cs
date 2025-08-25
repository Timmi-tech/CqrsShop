using Application.Features.Commands.AdjustStock;
using Application.Features.StockInventory.Queries;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/Inventory")]
    [Authorize]
    public class InventoryController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Adjust stock (increase)
        /// </summary>
        [HttpPost("increase")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> IncreaseStock([FromBody] IncreaseStockCommand command)
        {
            Result result = await _mediator.Send(command);
            return result.IsSuccess ? NoContent() : NotFound(result.Error);
        }
        /// <summary>
        /// Adjust stock (decrease)
        /// </summary>
        [HttpPost("decrease")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DecreaseStock([FromBody] DecreaseStockCommand command)
        {
            Result result = await _mediator.Send(command);
            return result.IsSuccess ? NoContent() : NotFound(result.Error);
        }

        /// <summary>
        /// Get stock level for a specific product
        /// </summary>
        [HttpGet("product/{productId:guid}")]
        public async Task<IActionResult> GetStockLevel(Guid productId)
        {
            int? stockLevel = await _mediator.Send(new GetStockLevelQuery(productId));
            return stockLevel.HasValue ? Ok(new { ProductId = productId, Quantity = stockLevel.Value }) : NotFound();
        }

        /// <summary>
        /// Get all stock levels
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllStockLevels()
        {
            List<StockLevelDto> stockLevels = await _mediator.Send(new GetAllStockLevelsQuery());
            return Ok(stockLevels);
        }
    }
}