using EshopWrapper.Core;
using EshopWrapper.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace EshopWrapper.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IEshopClient _client;

    public CustomersController(IEshopClient client)
    {
        _client = client;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomer(string id, [FromQuery] string? erpId)
    {
        var result = await _client.GetCustomerAsync(id, erpId);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetCustomerList([FromQuery] int numberOfDays)
    {
        var result = await _client.GetCustomerListAsync(numberOfDays);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddCustomer([FromBody] ExpandCustomer customer)
    {
        var result = await _client.AddCustomerAsync(customer);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCustomer([FromBody] ExpandCustomer customer)
    {
        var result = await _client.UpdateCustomerAsync(customer);
        return Ok(result);
    }
}
