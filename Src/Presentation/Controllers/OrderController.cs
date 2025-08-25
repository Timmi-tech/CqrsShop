using Application.DTOs;
using Application.Features.Orders.Commands.CancelOrder;
using Application.Features.Orders.Commands.CompletedOrder;
using Application.Features.Orders.Commands.CreateOrder;
using Application.Features.Orders.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/Order")]
    public class OrderController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        // Place an order
        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderCommand command)
        {
            var result = await _mediator.Send(command);

            return result.Match(
                onSuccess: orderId => CreatedAtAction(nameof(GetOrderById), new { orderId }, orderId) as IActionResult,
                onFailure: error => error.Code switch
                {
                    "NotFound" => NotFound(error),
                    "Validation" => BadRequest(error),
                    _ => StatusCode(error.StatusCode ?? 500, error)
                }
            );
        }
        // Cancel an order
        [HttpPut("{orderId}/cancel")]
        public async Task<IActionResult> CancelOrder(Guid orderId)
        {
            await _mediator.Send(new CancelOrderCommand(orderId));
            return NoContent();
        }

        // Complete an order
        [HttpPut("{orderId}/complete")]
        public async Task<IActionResult> CompleteOrder(Guid orderId)
        {
            await _mediator.Send(new CompletedOrderCommand(orderId));
            return NoContent();
        }

        // Get order by ID
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(Guid orderId)
        {
            OrderDto order = await _mediator.Send(new GetOrderByIdQuery(orderId));
            return Ok(order);
        }
        //  Get all orders
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            IEnumerable<OrderDto> orders = await _mediator.Send(new GetAllOrdersQuery());
            return Ok(orders);
        }

        // Get orders by user ID
        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetOrdersByUser(string userId)
        {
            List<OrderDto> orders = await _mediator.Send(new GetOrdersByUserQuery(userId));
            return Ok(orders);
        }

    }
}