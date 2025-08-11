using Application.Features.Orders.Commands.CancelOrder;
using Application.Features.Orders.Commands.CompletedOrder;
using Application.Features.Orders.Commands.CreateOrder;
using Application.Features.Orders.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/Order")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }
        // Place an order
        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderCommand command)
        {
            var orderId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetOrderById), new { orderId }, orderId);
        }

        // Cancel an order
        [HttpPut("{orderId}/cancel")]
        public async Task<IActionResult> CancelOrder(Guid orderId)
        {
            await _mediator.Send(new CancelOrderCommand(orderId));
            return NoContent();
        }
        // ✅ Complete an order
        [HttpPut("{orderId}/complete")]
        public async Task<IActionResult> CompleteOrder(Guid orderId)
        {
            await _mediator.Send(new CompletedOrderCommand(orderId));
            return NoContent();
        }
        // ✅ Get order by ID
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(Guid orderId)
        {
            var order = await _mediator.Send(new GetOrderByIdQuery(orderId));
            return Ok(order);
        }
        //  Get all orders
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _mediator.Send(new GetAllOrdersQuery());
            return Ok(orders);
        }

    }
}