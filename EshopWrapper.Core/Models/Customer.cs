using System.Text.Json.Serialization;

namespace EshopWrapper.Core.Models;

public class ExpandCustomer
{
    [JsonPropertyName("Customer")]
    public List<Customer>? CustomerList { get; set; }
}

public class Customer
{
    public string? CustomerNumber { get; set; }
    public string? CreatedDate { get; set; }
    public string? UserName { get; set; }
    public string? Company { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Street { get; set; }
    public string? StreetNumber { get; set; }
    public string? ApartmentNumber { get; set; }
    public string? FloorNumber { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? Zipcode { get; set; }
    public string? POBox { get; set; }
    public string? HomePhone { get; set; }
    public string? WorkPhone { get; set; }
    public string? MobilePhone { get; set; }
    public string? Fax { get; set; }
    public string? Email { get; set; }
    public string? IdentityCard { get; set; }
    public string? BirthDate { get; set; }
    public string? Newsletter { get; set; }
    public string? Gender { get; set; }
    public string? Language { get; set; }
    public string? InvoiceCustomerNumber { get; set; }
    public string? CompanyPosition { get; set; }
    public string? ErpNumber { get; set; }
    public string? ShippingAddress { get; set; }
    public string? DealerNumber { get; set; }
    public string? Set_points { get; set; }
}
