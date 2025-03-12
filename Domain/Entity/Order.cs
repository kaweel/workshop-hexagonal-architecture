namespace Domain.Entity;

public class Order
{
    public int Id { get; set; }
    public int CustomerID { get; set; }
    public string DeliveryChannel { get; set; } = string.Empty;
    public Discount? Discount { get; set; }
    public required List<ShopOrder> ShopOrders { get; set; }

    public decimal GetTotalAmount()
    {
        if (ShopOrders?.Any() != true)
        {
            return 0;
        }
        return ShopOrders.Sum(so => CalulateOrderItem(so.OrderItems));
    }

    public decimal CalulateOrderItem(List<OrderItem>? list)
    {
        if (list?.Any() != true)
        {
            return 0;
        }
        return list.Sum(i => (i.Price * i.Quantity) + CalAddOn(i.AddOns));
    }

    public decimal CalAddOn(List<AddOn>? list)
    {
        if (list?.Any() != true)
        {
            return 0;
        }
        return list.Sum(i => i.Price * i.Quantity);
    }

    public decimal GetDiscount()
    {
        if (Discount == null)
        {
            return 0;
        }
        return Discount.Operation switch
        {
            "%" => GetTotalAmount() * Discount.OperationAmount,
            "=" => Discount.OperationAmount,
            _ => 0
        };
    }

    public decimal ApplyDiscount()
    {
        var totalAmount = GetTotalAmount();
        var applyDiscount = Discount?.Condition switch
        {
            ">" => totalAmount > Discount.ConditionAmount,
            ">=" => totalAmount >= Discount.ConditionAmount,
            _ => false
        };
        return applyDiscount ? totalAmount - GetDiscount() : totalAmount;
    }

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
    public required Shop Shop { get; set; }
    public required List<OrderItem> OrderItems { get; set; }
}
public class OrderItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal Quantity { get; set; }
    public List<AddOn>? AddOns { get; set; }
    public string? Note { get; set; }
}
public class AddOn
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal Quantity { get; set; }
}
