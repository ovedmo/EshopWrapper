using System.Text.Json.Serialization;

namespace EshopWrapper.Core.Models;

public class Coupon
{
    [JsonPropertyName("general")]
    public List<CouponGeneral>? General { get; set; }

    [JsonPropertyName("products")]
    public List<CouponProduct>? Products { get; set; }

    [JsonPropertyName("categories")]
    public List<CouponCategory>? Categories { get; set; }
}

public class CouponGeneral
{
    [JsonPropertyName("coupon_code")]
    public string? CouponCode { get; set; }

    [JsonPropertyName("discount_value")]
    public string? DiscountValue { get; set; }

    [JsonPropertyName("discount_type")]
    public string? DiscountType { get; set; }

    [JsonPropertyName("coupon_type")]
    public string? CouponType { get; set; }

    [JsonPropertyName("expiration_date")]
    public string? ExpirationDate { get; set; }

    [JsonPropertyName("minimum_order")]
    public string? MinimumOrder { get; set; }

    [JsonPropertyName("reuse")]
    public string? Reuse { get; set; }

    [JsonPropertyName("coupon_name")]
    public string? CouponName { get; set; }

    [JsonPropertyName("erpid")]
    public string? ErpId { get; set; }
}

public class CouponProduct
{
    [JsonPropertyName("sku")]
    public string? Sku { get; set; }
}

public class CouponCategory
{
    [JsonPropertyName("category_id")]
    public string? CategoryId { get; set; }

    [JsonPropertyName("sub_category_id")]
    public string? SubCategoryId { get; set; }
}
