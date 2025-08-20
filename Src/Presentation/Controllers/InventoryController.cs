using Application.Features.Commands.AdjustStock;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/Inventory")]
    public class InventoryController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Adjust stock (increase)
        /// </summary>
        [HttpPost("increase")]
        public async Task<IActionResult> IncreaseStock([FromBody] IncreaseStockCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
        /// <summary>
        /// Adjust stock (decrease)
        /// </summary>
        [HttpPost("decrease")]
        public async Task<IActionResult> DecreaseStock([FromBody] DecreaseStockCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

    }
}