
using Application.Service;
using Application.Port;
using Application.DTO;
using Common.ApiException;
using Common.Provider;
using Moq;
using AutoMapper;
using Application.Mapping;

public class OrderApiAdapterGetOrderByIdTest
{
    private IOrderService _orderService;
    private IMapper _mapper;
    private Mock<IOrderRepositoryPort> _orderRepositoryPort;
    private Mock<IDateTimeProvider> _dateTimeProvider;
    private Order _order;

    public OrderApiAdapterGetOrderByIdTest()
    {
        _orderRepositoryPort = new();
        _dateTimeProvider = new();
        _mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingOrder>();
        }).CreateMapper();
        _orderService = new OrderService(_orderRepositoryPort.Object, _mapper, _dateTimeProvider.Object);

        _dateTimeProvider.Setup(m => m.UtcNow).Returns(new DateTime(2025, 2, 16, 0, 0, 0, DateTimeKind.Utc));

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
                    Shop = new Shop{
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
                                    Price = new decimal(20.00), Quantity = 1
                                }
                            }
                        }
                    }
                }
            },
            TotalAmount = 109,
            DiscountAmount = new decimal(10.900),
            ApplyDiscount = new decimal(98.100)

        };
        _orderRepositoryPort.Setup(m => m.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(_order);
    }

    [Fact]
    [Trait("Category", "UnitTest")]
    public async Task GetOrderById_When_FoundOrder_ShouldReturnsOrder()
    {
        var expected = await _orderService.GetOrderById(1);

        Assert.Equivalent(expected!.Discount, _order.Discount);
    }

    [Fact]
    [Trait("Category", "UnitTest")]
    public async Task GetOrderById_WhenNotFoundOrder_ShouldThrowNotFoundException()
    {
        _orderRepositoryPort.Setup(m => m.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Order?)null);

        var expected = await Assert.ThrowsAsync<NotFoundException>(() => _orderService.GetOrderById(1));

        Assert.Equal("order not found", expected.Message);
    }

    [Fact]
    [Trait("Category", "UnitTest")]
    public async Task GetOrderById_WhenUnhandleExceptionOccur_ShouldThrowException()
    {
        _orderRepositoryPort.Setup(m => m.GetByIdAsync(It.IsAny<int>())).ThrowsAsync(new Exception("unhandle exception"));

        var expected = await Assert.ThrowsAsync<Exception>(() => _orderService.GetOrderById(1));

        Assert.Equal("unhandle exception", expected.Message);
    }

}