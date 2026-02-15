using EshopWrapper.Core.Models;

namespace EshopWrapper.Core;

public interface IEshopClient
{
    // Coupon
    Task<object?> AddCouponAsync(Coupon coupon);
    Task<object?> GetCouponAsync(string couponCode);
    Task<object?> GetCouponListAsync(int numberOfDays);

    // Customer
    Task<Customer?> GetCustomerAsync(string? customerId = null, string? erpId = null);
    Task<List<Customer>?> GetCustomerListAsync(int numberOfDays);
    Task<object?> AddCustomerAsync(ExpandCustomer customer);
    Task<object?> UpdateCustomerAsync(ExpandCustomer customer);

    // Product
    Task<object?> AddProductAsync(ExpandProduct product); // Using the objectfull version
    Task<object?> UpdateProductAsync(ExpandProduct product, string? sku = null, string? erpNumber = null, string? language = null); // Using objectfull
    Task<Product?> GetProductAsync(string? itemId = null, string? erpId = null, string language = "he");
    Task<ExpandProduct?> GetProductFullAsync(string? itemId = null, string? erpId = null, string language = "he");
    Task<List<Product>?> GetProductListAsync(int numberOfDays = 0, string language = "he"); // Params from snippet were cut off but assuming similar pattern
    Task<object?> GetCategoriesAsync(string language = "he");

    // Order
    Task<ExpandOrder?> GetOrderAsync(int orderId);
    Task<List<object>?> GetOrderListAsync(int numberOfLastDays = 0, string fromDate = "", string toDate = "", int statusId = -1); // Return object for now as list items might differ
    Task<object?> CreateOrderAsync(ExpandOrder order);
    Task<object?> UpdateOrderAsync(ExpandOrder order, string? orderNumber = null, string? erpNumber = null);
    Task<object?> UpdateOrderStatusAsync(int orderId, int statusId);
}
