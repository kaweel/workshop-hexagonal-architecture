
using System.Text.Json.Serialization;
using Common.Helper;

namespace API.Entity
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public int CustomerID { get; set; }
        public required string DeliveryChannel { get; set; }
        public DiscountResponse? Discount { get; set; }
        public required List<ShopOrderResponse> Order { get; set; }
        
        // [JsonConverter(typeof(DecimalPrecisionConverter))]
        public decimal TotalAmount { get; set; } 
        public decimal DiscountAmount { get; set; }
        public decimal ApplyDiscount { get; set; }
    }
    public class DiscountResponse
    {
        public int Id { get; set; }
        public required string Code { get; set; }
        public required string Description { get; set; }
        public DateTime ExpireDate { get; set; }
    }
    public class ShopResponse
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
    public class ShopOrderResponse
    {
        public required ShopResponse Shop { get; set; }
        public required List<OrderItemResponse> OrderItem { get; set; }
    }
    public class OrderItemResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public List<AddOnResponse>? AddOn { get; set; }
        public string? Note { get; set; }
    }
    public class AddOnResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
    }
}
