using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Mssql.Entity;

[Table("Orders")]
public class Order
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Customer")]
    public int CustomerID { get; set; }

    [Required]
    [MaxLength(50)]
    public string DeliveryChannel { get; set; } = string.Empty;

    public int? DiscountId { get; set; }

    // Navigation Properties
    public virtual Discount? Discount { get; set; }
    public virtual List<ShopOrder> ShopOrders { get; set; } = new List<ShopOrder>();
    
}

[Table("Discounts")]
public class Discount
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    public string Operation { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal OperationAmount { get; set; }

    [Required]
    [MaxLength(10)]
    public string Condition { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal ConditionAmount { get; set; }

    public DateTime ExpireDate { get; set; }
}

[Table("Shops")]
public class Shop
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = string.Empty;
}

[Table("ShopOrders")]
public class ShopOrder
{
    public int OrderId { get; set; }
    public int ShopId { get; set; }
    // Navigation Properties
#pragma warning disable CS8618
    public virtual Order Order { get; set; }
    public virtual Shop Shop { get; set; }
#pragma warning restore CS8618
    public virtual List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}

[Table("OrderItems")]
public class OrderItem
{
    [Key]
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int ShopId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Quantity { get; set; }

    public string? Note { get; set; }

    // Navigation Properties
#pragma warning disable CS8618
    public virtual ShopOrder ShopOrder { get; set; }
#pragma warning restore CS8618
    public virtual List<AddOn> AddOns { get; set; } = new List<AddOn>();
}

[Table("AddOns")]
public class AddOn
{
    [Key]
    public int Id { get; set; }

    public int OrderItemId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Quantity { get; set; }

    // Navigation Property
#pragma warning disable CS8618
    public virtual OrderItem OrderItem { get; set; }
#pragma warning restore CS8618
}
