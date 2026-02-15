using EshopWrapper.Core;
using EshopWrapper.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace EshopWrapper.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IEshopClient _client;

    public OrdersController(IEshopClient client)
    {
        _client = client;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        var result = await _client.GetOrderAsync(id);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetOrderList([FromQuery] int numberOfLastDays = 0, [FromQuery] string fromDate = "", [FromQuery] string toDate = "", [FromQuery] int statusId = -1)
    {
        var result = await _client.GetOrderListAsync(numberOfLastDays, fromDate, toDate, statusId);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] ExpandOrder order)
    {
        var result = await _client.CreateOrderAsync(order);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateOrder([FromBody] ExpandOrder order, [FromQuery] string? orderNumber, [FromQuery] string? erpNumber)
    {
        var result = await _client.UpdateOrderAsync(order, orderNumber, erpNumber);
        return Ok(result);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] int statusId)
    {
        var result = await _client.UpdateOrderStatusAsync(id, statusId);
        return Ok(result);
    }
}
