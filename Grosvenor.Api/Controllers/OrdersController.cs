using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Grosvenor.Api.Controllers;

[ApiController]
[Route("orders")]
public class OrdersController(IServer server) : ControllerBase
{
    /// <summary>
    /// Takes an order and returns the result.
    /// </summary>
    /// <param name="order">The order string, e.g., "morning, 1, 2, 3".</param>
    /// <returns>The result of the order.</returns>
    [HttpGet]
    public IActionResult TakeOrder(string order)
    {
        var result = server.TakeOrder(order);
        return Ok(result);
    }
}