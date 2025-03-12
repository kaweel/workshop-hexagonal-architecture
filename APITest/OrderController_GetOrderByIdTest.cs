using System.Net;
using System.Net.Http.Json;
using API.Entity;
using Application.Port;
using Common.ApiException;
using Application.DTO;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

public class OrderController_GetOrderByIdTest : TestControllerBase
{
    public OrderController_GetOrderByIdTest(WebApplicationFactory<Program> factory) : base(factory.WithWebHostBuilder(builder =>
    {
        var mockIOrderPort = new Mock<IOrderServicePort>();
        var order = new Order
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
                                    Price = new decimal(20.00),
                                    Quantity = 1
                                }
                            }
                        }
                    }
                    }
                },
            TotalAmount = 109,
            DiscountAmount = new decimal(10.9),
            ApplyDiscount = new decimal(98.1)
        };
        mockIOrderPort.Setup(m => m.GetOrderById(It.IsAny<int>())).ReturnsAsync(order);
        mockIOrderPort.Setup(m => m.GetOrderById(2)).ThrowsAsync(new NotFoundException("order not found"));
        mockIOrderPort.Setup(m => m.GetOrderById(3)).ThrowsAsync(new Exception("Unhandle exception"));
        builder.ConfigureServices(s =>
        {
            s.AddSingleton(mockIOrderPort.Object);
        });
    }))
    { }

    [Fact]
    [Trait("Category", "UnitTest")]
    public async Task GetOrderById_When_Found_Order_Should_Returns_Ok()
    {
        var expectedBody = new OrderResponse
        {
            Id = 1,
            CustomerID = 1,
            DeliveryChannel = "delivery",
            Discount = new DiscountResponse
            {
                Id = 1,
                Code = "JD20",
                Description = "ใจดี 20%",
                ExpireDate = new DateTime(2025, 2, 16, 0, 0, 0, DateTimeKind.Utc),
            },
            Order = new List<ShopOrderResponse> {
                new ShopOrderResponse {
                    Shop = new ShopResponse{
                            Id = 1,
                            Code = "0001",
                            Name = "Thai Bubble Tea",
                            Status = "Active",
                    },
                    OrderItem = new List<OrderItemResponse> {
                        new OrderItemResponse {
                            Id = 1,
                            Name = "Milk Tea",
                            Price = 89.00m,
                            Quantity = 1,
                            AddOn = new List<AddOnResponse> {
                                new AddOnResponse {
                                    Id = 1,
                                    Name = "Singature Bubble",
                                    Price = new decimal(20.00),
                                    Quantity = 1
                                }
                            }
                        }
                    }
                }
            },

            TotalAmount = new decimal(109.00),
            DiscountAmount = new decimal(10.90),
            ApplyDiscount = new decimal(98.100),

        };

        var actual = await _client.GetAsync("/api/orders/1");

        var actualBody = await actual.Content.ReadFromJsonAsync<OrderResponse>();

        Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        Assert.Equivalent(expectedBody, actualBody);
    }

    [Fact]
    [Trait("Category", "UnitTest")]
    public async Task GetOrderById_When_Not_Found_Order_Should_Returns_NotFound()
    {
        var actual = await _client.GetAsync("/api/orders/2");

        Assert.Equal(HttpStatusCode.NotFound, actual.StatusCode);
    }

    [Fact]
    [Trait("Category", "UnitTest")]
    public async Task GetOrderById_When_Unhandle_Exception_Occur_Should_Returns_Internal_Error()
    {
        var actual = await _client.GetAsync("/api/orders/3");

        Assert.Equal(HttpStatusCode.InternalServerError, actual.StatusCode);
    }

}
