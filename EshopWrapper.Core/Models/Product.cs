using System.Text.Json.Serialization;

namespace EshopWrapper.Core.Models;

public class ExpandProduct
{
    [JsonPropertyName("General")]
    public List<ProductGeneral>? General { get; set; }

    [JsonPropertyName("categories")]
    public List<ProductCategory>? Categories { get; set; }

    [JsonPropertyName("Prices")]
    public List<ProductPrice>? Prices { get; set; }

    [JsonPropertyName("Images")]
    public List<ProductImage>? Images { get; set; }

    [JsonPropertyName("Attributes")]
    public List<ItemProperty>? Attributes { get; set; }

    [JsonPropertyName("Filters")]
    public List<MiscField>? Filters { get; set; }

    [JsonPropertyName("SEO")]
    public List<ProductSeo>? Seo { get; set; }

    [JsonPropertyName("RelatedItems")]
    public List<ItemConnection>? RelatedItems { get; set; }
}

public class ProductGeneral
{
    public string? SKU { get; set; }
    public string? ERPid { get; set; }
    public string? Name { get; set; }
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }
    public int? PaymentsNoIntrest { get; set; }
    public int? DeliveryTime { get; set; }
    public int? Warranty { get; set; }
    public string? Manufacturer { get; set; }
    public string? Model { get; set; }
    public int? Condition { get; set; }
    public int? Inventory { get; set; }
    public float? Weight { get; set; }
    public int? Status { get; set; }
    public string? ZapProductURL { get; set; }
    public string? Langcode { get; set; }
    public string? WarrantyText { get; set; }
    public int? MaxPayments { get; set; }
    public string? SupplierName { get; set; }
    public string? ImportSource { get; set; }
    public string? Barcode { get; set; }
    public string? itemcost { get; set; }
}

public class ProductCategory
{
    public string? CategoryName { get; set; }
    public string? SubcategoryName { get; set; }
    public bool? Parent { get; set; }
}

public class ProductPrice
{
    public int? CurrencyId { get; set; }
    public string? DealerName { get; set; }
    public float? SalePrice { get; set; }
    public float? RegularPrice { get; set; }
    public float? DeliveryPrice { get; set; }
}

public class ProductImage
{
    public int? ID { get; set; }
    public string? URL { get; set; }
    public string? Type { get; set; }
    public string? Alt { get; set; }
}

public class ItemProperty
{
    public string? PropertyName { get; set; }
    public string? OptionName { get; set; }
    public float? Price { get; set; }
    public int? Mandatory { get; set; }
    public string? ViewType { get; set; }
}

public class MiscField
{
    public string? Name { get; set; }
    public string? DataTypeValue { get; set; }
    public bool? Mandatory { get; set; }
    public string? Value { get; set; }
    public string? CategoryName { get; set; }
    public string? SubcategoryName { get; set; }
}

public class ProductSeo
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Keywords { get; set; }
    public string? Source { get; set; }
}

public class ItemConnection
{
    public string? SKU { get; set; }
    public string? Display { get; set; }
    public int? Discount { get; set; }
    public string? DiscountType { get; set; }
}

public class Product // The simpler summary model
{
    public string? SKU { get; set; }
    public int? ProductNumber { get; set; }
    public int? CategoryNumber { get; set; }
    public int? SubCategoryNumber { get; set; }
    public string? Name { get; set; }
    public string? ShortDescription { get; set; }
    public int? PaymentsNoIntrest { get; set; }
    public int? DeliveryTime { get; set; }
    public int? Warranty { get; set; }
    public string? Manufacturer { get; set; }
    public string? Model { get; set; }
    public float? DeliveryPrice { get; set; }
    public int? Condition { get; set; }
    public float? RegularPrice { get; set; }
    public float? SalePrice { get; set; }
    public int? Inventory { get; set; }
    public string? ZapProductURL { get; set; }
    public string? LangCode { get; set; }
    public int? Status { get; set; }
    public string? ErpID { get; set; }
}
