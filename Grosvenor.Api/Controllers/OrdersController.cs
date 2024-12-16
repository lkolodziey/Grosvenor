using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Grosvenor.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController(IServer server) : ControllerBase
{
    /// <summary>
    /// Takes an order and returns the result.
    /// </summary>
    /// <param name="unparsedOrder">The order string, e.g., "morning, 1, 2, 3".</param>
    /// <returns>The result of the order.</returns>
    [HttpGet]
    public IActionResult TakeOrder(string unparsedOrder)
    {
        var result = server.TakeOrder(unparsedOrder);
        return Ok(result);
    }
}