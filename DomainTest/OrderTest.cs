using Domain.Entity;

namespace DomainTest;

public class OrderTest
{
    private readonly Order _order;
    public OrderTest()
    {
        _order = new Order
        {
            Id = 1,
            CustomerID = 1,
            DeliveryChannel = "delivery",
            Discount = new Discount
            {
                Id = 1,
                Code = "JD20",
                Description = "ใจดี 20%",
                Operation = "%",
                OperationAmount = new decimal(0.1),
                Condition = ">",
                ConditionAmount = 100,
                ExpireDate = new DateTime(2025, 2, 16, 0, 0, 0, DateTimeKind.Utc),
            },
            ShopOrders = new List<ShopOrder> {
                new ShopOrder {
                    Shop = new Shop {
                        Id = 1,
                        Code = "0001",
                        Name = "Thai Bubble Tea",
                        Status = "Active",
                    },
                    OrderItems = new List<OrderItem> {
                        new OrderItem {
                            Id = 1,
                            Name = "Milk Tea",
                            Price = 89.00m,
                            Quantity = 1,
                            AddOns = new List<AddOn> {
                                new AddOn {
                                    Id = 1,
                                    Name = "Singature Bubble",
                                    Price = new decimal(20.00),
                                    Quantity = 1
                                }
                            }
                        }
                    }
                }
            }
        };
    }

    [Fact]
    [Trait("Category", "UnitTest")]
    public void GetTotalAmount_WhenNoOrder_ShouldZero()
    {
        _order.ShopOrders = [];

        var actual = _order.GetTotalAmount();

        Assert.Equal(0, actual);
    }

    [Fact]
    [Trait("Category", "UnitTest")]
    public void GetTotalAmount_WithMultipleOrder_ShouldReturnTotalAmount()
    {
        var actual = _order.GetTotalAmount();

        Assert.Equal(109, actual);
    }

    [Fact]
    [Trait("Category", "UnitTest")]
    public void GetDiscount_WhenHaveNoDiscount_ShouldReturnZero()
    {
        _order.Discount = null;

        var actual = _order.GetDiscount();

        Assert.Equal(0, actual);
    }

    [Fact]
    [Trait("Category", "UnitTest")]
    public void GetDiscount_WhenInvalidOperate_ShouldReturnZero()
    {
        _order.Discount!.Operation = "x";

        var actual = _order.GetDiscount();

        Assert.Equal(0, actual);
    }

    [Theory]
    [InlineData("%", 0.1, 10.9)]
    [InlineData("=", 10, 10)]
    [Trait("Category", "UnitTest")]
    public void GetDiscount_WhenValidOperate_ShouldReturnDiscount(string operation, decimal amount, decimal expected)
    {
        _order.Discount!.Operation = operation;
        _order.Discount!.OperationAmount = amount;

        var actual = _order.GetDiscount();

        Assert.Equal(expected, actual);
    }


    [Theory]
    [InlineData(">", 108, 98.1)]
    [InlineData(">=", 109, 98.1)]
    [Trait("Category", "UnitTest")]
    public void ApplyDiscount_WhenValidCondition_ShouldTotalAmountWithDiscount(string operation, decimal amount, decimal expected)
    {
        _order.Discount!.Condition = operation;
        _order.Discount!.ConditionAmount = amount;

        var actual = _order.ApplyDiscount();

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(">", 110, 109)]
    [InlineData(">=", 110, 109)]
    [InlineData("-", 110, 109)]
    [Trait("Category", "UnitTest")]
    public void ApplyDiscount_WhenInvalidCondition_ShouldTotalAmountWithoutDiscount(string operation, decimal amount, decimal expected)
    {
        _order.Discount!.Condition = operation;
        _order.Discount!.ConditionAmount = amount;

        var actual = _order.ApplyDiscount();

        Assert.Equal(expected, actual);
    }

}
