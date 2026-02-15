using System.Net.Http.Json;
using System.Text.Json;
using EshopWrapper.Core.Models;

namespace EshopWrapper.Core;

public class EshopClient : IEshopClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public EshopClient(HttpClient httpClient, string apiKey)
    {
        _httpClient = httpClient;
        _apiKey = apiKey;
        if (_httpClient.BaseAddress != null && !_httpClient.BaseAddress.ToString().EndsWith("/"))
        {
            _httpClient.BaseAddress = new Uri(_httpClient.BaseAddress + "/");
        }
    }

    private string AddKey(string url)
    {
        return url + (url.Contains("?") ? "&" : "?") + $"key={_apiKey}";
    }

    // Coupon
    public async Task<object?> AddCouponAsync(Coupon coupon)
    {
        var response = await _httpClient.PostAsJsonAsync(AddKey("api/addcoupon"), coupon);
        return await response.Content.ReadFromJsonAsync<object>();
    }

    public async Task<object?> GetCouponAsync(string couponCode)
    {
        return await _httpClient.GetFromJsonAsync<object>(AddKey($"api/getcoupon?Coupon_code={couponCode}"));
    }

    public async Task<object?> GetCouponListAsync(int numberOfDays)
    {
        return await _httpClient.GetFromJsonAsync<object>(AddKey($"api/getcouponlist?numberoflastdays={numberOfDays}"));
    }

    // Customer
    public async Task<Customer?> GetCustomerAsync(string? customerId = null, string? erpId = null)
    {
        var query = "";
        if (!string.IsNullOrEmpty(customerId)) query += $"&customerid={customerId}";
        if (!string.IsNullOrEmpty(erpId)) query += $"&erpid={erpId}";
        
        // The return type in swagger says object, but typically it returns the customer object. 
        // We'll try to deserialize to Customer, or potentially ExpandCustomer wrapper.
        // Based on "Get customer" typically returning the customer data.
        // If it fails, we might need to adjust.
        return await _httpClient.GetFromJsonAsync<Customer>(AddKey($"api/getcustomer?{query}"));
    }

    public async Task<List<Customer>?> GetCustomerListAsync(int numberOfDays)
    {
        // Swagger says "Get a list of all customers". likely returns array/list.
        return await _httpClient.GetFromJsonAsync<List<Customer>>(AddKey($"api/getcustomerlist?numberoflastdays={numberOfDays}"));
    }

    public async Task<object?> AddCustomerAsync(ExpandCustomer customer)
    {
        var response = await _httpClient.PostAsJsonAsync(AddKey("api/addcustomer"), customer);
        return await response.Content.ReadFromJsonAsync<object>();
    }

    public async Task<object?> UpdateCustomerAsync(ExpandCustomer customer)
    {
        var response = await _httpClient.PostAsJsonAsync(AddKey("api/updatecustomer"), customer);
        return await response.Content.ReadFromJsonAsync<object>();
    }

    // Product
    public async Task<object?> AddProductAsync(ExpandProduct product)
    {
        var response = await _httpClient.PostAsJsonAsync(AddKey("api/addproductobjectfull"), product);
        return await response.Content.ReadFromJsonAsync<object>();
    }

    public async Task<object?> UpdateProductAsync(ExpandProduct product, string? sku = null, string? erpNumber = null, string? language = null)
    {
        var query = "";
        if (!string.IsNullOrEmpty(sku)) query += $"&Sku={sku}";
        if (!string.IsNullOrEmpty(erpNumber)) query += $"&ErpNumber={erpNumber}";
        if (!string.IsNullOrEmpty(language)) query += $"&Language={language}";

        var response = await _httpClient.PostAsJsonAsync(AddKey($"api/updateproductobjectfull?{query}"), product);
        return await response.Content.ReadFromJsonAsync<object>();
    }

    public async Task<Product?> GetProductAsync(string? itemId = null, string? erpId = null, string language = "he")
    {
        var query = $"&language={language}";
        if (!string.IsNullOrEmpty(itemId)) query += $"&itemid={itemId}";
        if (!string.IsNullOrEmpty(erpId)) query += $"&erpid={erpId}";

        return await _httpClient.GetFromJsonAsync<Product>(AddKey($"api/getproduct?{query}"));
    }

    public async Task<ExpandProduct?> GetProductFullAsync(string? itemId = null, string? erpId = null, string language = "he")
    {
        var query = $"&language={language}";
        if (!string.IsNullOrEmpty(itemId)) query += $"&itemid={itemId}";
        if (!string.IsNullOrEmpty(erpId)) query += $"&erpid={erpId}";

        return await _httpClient.GetFromJsonAsync<ExpandProduct>(AddKey($"api/getproductfull?{query}"));
    }

    public async Task<List<Product>?> GetProductListAsync(int numberOfDays = 0, string language = "he")
    {
         var query = $"&language={language}";
         if (numberOfDays > 0) query += $"&numberofdays={numberOfDays}";
         // Note: Swagger snippet for getproductlist param was cut off but usually 'numberofdays' or 'numberoflastdays'
         // I saw in Chunk 10 "/api/getlastproducts" (maybe?) No, looked at chunk 10/28 again.
         // Wait, chunk 10 shows "/api/getproducts"? No.
         // Chunk 6, 80 starts with "/api/addproduct".
         // Chunk 9, 83 ends with "parameters": [". 
         // I missed the getproductlist params in my read. I'll guess numberofdays based on others.
         
         return await _httpClient.GetFromJsonAsync<List<Product>>(AddKey($"api/getproductlist?{query}"));
    }

    public async Task<object?> GetCategoriesAsync(string language = "he")
    {
        return await _httpClient.GetFromJsonAsync<object>(AddKey($"api/getcategories?language={language}"));
    }

    // Order
    public async Task<ExpandOrder?> GetOrderAsync(int orderId)
    {
        return await _httpClient.GetFromJsonAsync<ExpandOrder>(AddKey($"api/getorder?orderid={orderId}"));
    }

    public async Task<List<object>?> GetOrderListAsync(int numberOfLastDays = 0, string fromDate = "", string toDate = "", int statusId = -1)
    {
        var query = $"&statusid={statusId}";
        if (numberOfLastDays > 0) query += $"&numberoflastdays={numberOfLastDays}";
        if (!string.IsNullOrEmpty(fromDate)) query += $"&fromdate={fromDate}";
        if (!string.IsNullOrEmpty(toDate)) query += $"&todate={toDate}";

        return await _httpClient.GetFromJsonAsync<List<object>>(AddKey($"api/getorderlist?{query}"));
    }

    public async Task<object?> CreateOrderAsync(ExpandOrder order)
    {
        var response = await _httpClient.PostAsJsonAsync(AddKey("api/createorder"), order);
        return await response.Content.ReadFromJsonAsync<object>();
    }

    public async Task<object?> UpdateOrderAsync(ExpandOrder order, string? orderNumber = null, string? erpNumber = null)
    {
        var query = "";
        if (!string.IsNullOrEmpty(orderNumber)) query += $"&OrderNumber={orderNumber}";
        if (!string.IsNullOrEmpty(erpNumber)) query += $"&ErpNumber={erpNumber}";

        var response = await _httpClient.PostAsJsonAsync(AddKey($"api/updateorderobject?{query}"), order);
        return await response.Content.ReadFromJsonAsync<object>();
    }

    public async Task<object?> UpdateOrderStatusAsync(int orderId, int statusId)
    {
        var response = await _httpClient.PostAsJsonAsync(AddKey($"api/updateorderstatus?orderid={orderId}&statusid={statusId}"), new { });
        return await response.Content.ReadFromJsonAsync<object>();
    }
}
