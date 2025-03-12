namespace Application.DTO;

public class Order
{
    public int Id { get; set; }
    public int CustomerID { get; set; }

    public string DeliveryChannel { get; set; } = string.Empty;

    public int? DiscountId { get; set; }

    public Discount? Discount { get; set; }
    public List<ShopOrder>? ShopOrders { get; set; }

    public decimal TotalAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal ApplyDiscount { get; set; }
}

public class Discount
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Operation { get; set; } = string.Empty;
    public decimal OperationAmount { get; set; }
    public string Condition { get; set; } = string.Empty;
    public decimal ConditionAmount { get; set; }
    public DateTime ExpireDate { get; set; }
}

public class Shop
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class ShopOrder
{
    public int OrderId { get; set; }
    public int ShopId { get; set; }
    public Order? Order { get; set; }
    public Shop? Shop { get; set; }
    public List<OrderItem>? OrderItems { get; set; }
}

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ShopId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal Quantity { get; set; }
    public string? Note { get; set; }
    public ShopOrder? ShopOrder { get; set; }
    public List<AddOn>? AddOns { get; set; }
}

public class AddOn
{
    public int Id { get; set; }
    public int OrderItemId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal Quantity { get; set; }
    public OrderItem? OrderItem { get; set; }
}
