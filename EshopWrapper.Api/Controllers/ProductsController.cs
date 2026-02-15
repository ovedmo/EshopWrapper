using EshopWrapper.Core;
using EshopWrapper.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace EshopWrapper.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IEshopClient _client;

    public ProductsController(IEshopClient client)
    {
        _client = client;
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] ExpandProduct product)
    {
        var result = await _client.AddProductAsync(product);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProduct([FromBody] ExpandProduct product, [FromQuery] string? sku, [FromQuery] string? erpNumber, [FromQuery] string language = "he")
    {
        var result = await _client.UpdateProductAsync(product, sku, erpNumber, language);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(string id, [FromQuery] string? erpId, [FromQuery] string language = "he")
    {
        var result = await _client.GetProductAsync(id, erpId, language);
        return Ok(result);
    }

    [HttpGet("{id}/full")]
    public async Task<IActionResult> GetProductFull(string id, [FromQuery] string? erpId, [FromQuery] string language = "he")
    {
        var result = await _client.GetProductFullAsync(id, erpId, language);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetProductList([FromQuery] int numberOfDays = 0, [FromQuery] string language = "he")
    {
        var result = await _client.GetProductListAsync(numberOfDays, language);
        return Ok(result);
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories([FromQuery] string language = "he")
    {
        var result = await _client.GetCategoriesAsync(language);
        return Ok(result);
    }
}
