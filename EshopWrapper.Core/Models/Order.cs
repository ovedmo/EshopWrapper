using System.Text.Json.Serialization;

namespace EshopWrapper.Core.Models;

public class ExpandOrder
{
    [JsonPropertyName("Order")]
    public List<OrderGeneral>? Order { get; set; }

    [JsonPropertyName("Billing")]
    public List<OrderBilling>? Billing { get; set; }

    [JsonPropertyName("Delivery")]
    public List<OrderDelivery>? Delivery { get; set; }

    [JsonPropertyName("Comments")]
    public List<OrderComments>? Comments { get; set; }

    [JsonPropertyName("Supplier")]
    public List<OrderSupplier>? Supplier { get; set; }

    [JsonPropertyName("Products")]
    public List<OrderProduct>? Products { get; set; }

    [JsonPropertyName("ShippingAddress")]
    public List<OrderShippingAddress>? ShippingAddress { get; set; }

    [JsonPropertyName("BillingAddress")]
    public List<OrderBillingAddress>? BillingAddress { get; set; }
}

public class OrderGeneral
{
    public string? StatusNumber { get; set; }
    public string? LeadStatus { get; set; }
    public string? Total { get; set; }
    public string? Discount { get; set; }
    public string? Tax { get; set; }
    public string? Interest { get; set; }
    public string? Currency { get; set; }
    public string? ErpNumber { get; set; }
    public string? ErpUrl { get; set; }
    public string? SalesManNumber { get; set; }
}

public class OrderBilling
{
    public string? BillingStatus { get; set; }
    public string? PaymentMethod { get; set; }
    public string? Payments { get; set; }
    public string? CreditCardApproveNumber { get; set; }
    public string? CreditCardJ5AuthNumber { get; set; }
    public string? CreditCardTransactionLog { get; set; }
    public string? CreditCardTransactionNumber { get; set; }
    public string? InvoiceNumber { get; set; }
    public string? InvoiceInTheNameOf { get; set; }
    public string? InvoiceUrl { get; set; }
    public string? InvoiceLog { get; set; }
    public string? InvoiceCode { get; set; }
}

public class OrderDelivery
{
    public string? DeliveryCompanyNumber { get; set; }
    public string? DeliveryCompanyPrice { get; set; }
    public string? DeliveryCompanyCost { get; set; }
    public string? Delivery { get; set; }
    public string? DeliveryDate { get; set; }
    public string? DeliveryHours { get; set; }
    public string? DeliveryNumber { get; set; }
    public string? DeliveryPrice { get; set; }
    public string? PickupLocation { get; set; }
    public string? Pickup { get; set; }
}

public class OrderComments
{
    public string? SuppliersComments { get; set; }
    public string? AdminComments { get; set; }
    public string? StoreMessage { get; set; }
    public string? CustomerMessage { get; set; }
    public string? DeliveryComments { get; set; }
}

public class OrderSupplier
{
    public string? SupplierNumber { get; set; }
    public string? SupplierType { get; set; }
    public string? SupplierCost { get; set; }
    public string? SupplierPrice { get; set; }
}

public class OrderProduct
{
    public string? OrderItemID { get; set; }
    public string? ProductNumber { get; set; }
    public string? SKU { get; set; }
    public string? Name { get; set; }
    public string? Attributes { get; set; }
    public string? Price { get; set; }
    public string? Quantity { get; set; }
    public string? CouponDiscount { get; set; }
    public string? Coupon { get; set; }
    public string? Manufacturer { get; set; }
    public string? CategoryNumber { get; set; }
    public string? SubcategoryNumber { get; set; }
    public string? StatusNumber { get; set; }
    public string? StatusName { get; set; }
    public string? ParentProductSKU { get; set; }
    public string? StickyItemSKU { get; set; }
    public string? ErpNumber { get; set; }
}

public class OrderShippingAddress
{
    public string? ShippingCompanyName { get; set; }
    public string? ShippingFirstName { get; set; }
    public string? ShippingLastName { get; set; }
    public string? ShippingAddress { get; set; }
    public string? ShippingStreet { get; set; }
    public string? ShippingStreetNumber { get; set; }
    public string? ShippingApartmentNumber { get; set; }
    public string? ShippingFloorNumber { get; set; }
    public string? ShippingCity { get; set; }
    public string? ShippingState { get; set; }
    public string? ShippingCountry { get; set; }
    public string? ShippingZip { get; set; }
    public string? ShippingPobox { get; set; }
    public string? ShippingHomePhone { get; set; }
    public string? ShippingWorkPhone { get; set; }
    public string? ShippingMobilePhone { get; set; }
}

public class OrderBillingAddress
{
    public string? BillingCompanyName { get; set; }
    public string? BillingFirstName { get; set; }
    public string? BillingLastName { get; set; }
    public string? BillingStreet { get; set; }
    public string? BillingStreetNumber { get; set; }
    public string? BillingApartmentNumber { get; set; }
    public string? BillingFloorNumber { get; set; }
    public string? BillingCity { get; set; }
    public string? BillingZipcode { get; set; }
    public string? BillingState { get; set; }
    public string? BillingCountry { get; set; }
    public string? BillingHomePhone { get; set; }
    public string? BillingWorkPhone { get; set; }
    public string? BillingMobilePhone { get; set; }
    public string? BillingEmail { get; set; }
    public string? CustomerNumber { get; set; }
}
