namespace API.Entity
{
    public abstract class BaseOrderRequest
    {
        public int CustomerID { get; set; }
        public required string DeliveryChannel { get; set; }
        public int? DiscountID { get; set; }
        public required List<ShopOrderRequest> Order { get; set; }
    }

    public class CreateOrderRequest : BaseOrderRequest { }
    public class UpdateOrderRequest : BaseOrderRequest
    {
        public int Id { get; set; }
    }
    public class ShopOrderRequest
    {
        public int ShopID { get; set; }
        public required List<OrderItemRequest> OrderItem { get; set; }
    }
    public class OrderItemRequest
    {
        public int Id { get; set; }
        public List<AddOnRequest>? AddOn { get; set; }
        public string? Note { get; set; }
    }
    public class AddOnRequest
    {
        public int Id { get; set; }
    }
}
