
using AutoMapper;
using Infrastructure.Mapping;

namespace InfrastructureTest;

public class MappingOrderEntityToDtoTest
{
    private readonly Infrastructure.Mssql.Entity.Order _mssqlEntityOrder;
    private readonly Application.DTO.Order _expectApplicationDtoOrder;
    private readonly Application.DTO.Order _actualApplicationDtoOrder;
    private readonly IMapper _mapper;
    public MappingOrderEntityToDtoTest()
    {
        _mssqlEntityOrder = new Infrastructure.Mssql.Entity.Order
        {
            CustomerID = 1,
            DeliveryChannel = "delivery",
            Discount = new Infrastructure.Mssql.Entity.Discount
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
            ShopOrders = new List<Infrastructure.Mssql.Entity.ShopOrder> {
            new Infrastructure.Mssql.Entity.ShopOrder {
                    ShopId = 1,
                    OrderId = 1,
                    Shop = new Infrastructure.Mssql.Entity.Shop{
                        Id = 1,
                        Code = "001",
                        Name = ".NET Thailand",
                        Status = "Active",
                    },
                    OrderItems = new List<Infrastructure.Mssql.Entity.OrderItem> {
                        new Infrastructure.Mssql.Entity.OrderItem {
                            Id = 1,
                            Name = "Milk Tea",
                            Price = 89.00m,
                            Quantity = 1,
                            AddOns = new List<Infrastructure.Mssql.Entity.AddOn> {
                                new Infrastructure.Mssql.Entity.AddOn {
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
        _expectApplicationDtoOrder = new Application.DTO.Order
        {
            CustomerID = 1,
            DeliveryChannel = "delivery",
            Discount = new Application.DTO.Discount
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
            ShopOrders = new List<Application.DTO.ShopOrder> {
            new Application.DTO.ShopOrder {
                    Shop = new Application.DTO.Shop{
                        Id = 1,
                        Code = "001",
                        Name = ".NET Thailand",
                        Status = "Active",
                    },
                    OrderItems = new List<Application.DTO.OrderItem> {
                        new Application.DTO.OrderItem {
                            Id = 1,
                            Name = "Milk Tea",
                            Price = 89.00m,
                            Quantity = 1,
                            AddOns = new List<Application.DTO.AddOn> {
                                new Application.DTO.AddOn {
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
        _mapper = new MapperConfiguration(cfg => { cfg.AddProfile<MappingOrderEntityToDto>(); }).CreateMapper();
        _actualApplicationDtoOrder = _mapper.Map<Application.DTO.Order>(_mssqlEntityOrder);
    }

    [Fact]
    [Trait("Category", "UnitTest")]
    public void MappingMssqlOrderEntityToApplicationOrderDto()
    {
        Assert.Equal(_expectApplicationDtoOrder.Id, _actualApplicationDtoOrder.Id);
        Assert.Equal(_expectApplicationDtoOrder.CustomerID, _actualApplicationDtoOrder.CustomerID);
        Assert.Equal(_expectApplicationDtoOrder.DeliveryChannel, _actualApplicationDtoOrder.DeliveryChannel);
    }

    [Fact]
    [Trait("Category", "UnitTest")]
    public void MappingMssqlOrderDiscountEntityToApplicationOrderDiscountDto()
    {
        Assert.NotNull(_actualApplicationDtoOrder.Discount);
        Assert.Equal(_expectApplicationDtoOrder.Discount!.Id, _actualApplicationDtoOrder.Discount!.Id);
        Assert.Equal(_expectApplicationDtoOrder.Discount.Code, _actualApplicationDtoOrder.Discount.Code);
        Assert.Equal(_expectApplicationDtoOrder.Discount.Description, _actualApplicationDtoOrder.Discount.Description);
        Assert.Equal(_expectApplicationDtoOrder.Discount.Operation, _actualApplicationDtoOrder.Discount.Operation);
        Assert.Equal(_expectApplicationDtoOrder.Discount.OperationAmount, _actualApplicationDtoOrder.Discount.OperationAmount);
        Assert.Equal(_expectApplicationDtoOrder.Discount.Condition, _actualApplicationDtoOrder.Discount.Condition);
        Assert.Equal(_expectApplicationDtoOrder.Discount.ConditionAmount, _actualApplicationDtoOrder.Discount.ConditionAmount);
        Assert.Equal(_expectApplicationDtoOrder.Discount.ExpireDate, _actualApplicationDtoOrder.Discount.ExpireDate);
    }

    [Fact]
    [Trait("Category", "UnitTest")]
    public void MappingMssqlOrderShopEntityToApplicationOrderShopDto()
    {
#pragma warning disable CS8602
        Assert.NotNull(_actualApplicationDtoOrder.ShopOrders[0]);
        Assert.Equal(_expectApplicationDtoOrder.ShopOrders[0].Shop.Id, _actualApplicationDtoOrder.ShopOrders[0].Shop.Id);
        Assert.Equal(_expectApplicationDtoOrder.ShopOrders[0].Shop.Code, _actualApplicationDtoOrder.ShopOrders[0].Shop.Code);
        Assert.Equal(_expectApplicationDtoOrder.ShopOrders[0].Shop.Name, _actualApplicationDtoOrder.ShopOrders[0].Shop.Name);
        Assert.Equal(_expectApplicationDtoOrder.ShopOrders[0].Shop.Status, _actualApplicationDtoOrder.ShopOrders[0].Shop.Status);
#pragma warning disable CS8602
    }

    [Fact]
    [Trait("Category", "UnitTest")]
    public void MappingMssqlOrderListOrderItemEntityToApplicationOrderListOrderItemDto()
    {
#pragma warning disable CS8602
        Assert.NotNull(_actualApplicationDtoOrder.ShopOrders[0].OrderItems[0]);
        Assert.Equal(_expectApplicationDtoOrder.ShopOrders[0].OrderItems[0].Id, _actualApplicationDtoOrder.ShopOrders[0].OrderItems[0].Id);
        Assert.Equal(_expectApplicationDtoOrder.ShopOrders[0].OrderItems[0].Name, _actualApplicationDtoOrder.ShopOrders[0].OrderItems[0].Name);
        Assert.Equal(_expectApplicationDtoOrder.ShopOrders[0].OrderItems[0].Price, _actualApplicationDtoOrder.ShopOrders[0].OrderItems[0].Price);
        Assert.Equal(_expectApplicationDtoOrder.ShopOrders[0].OrderItems[0].Quantity, _actualApplicationDtoOrder.ShopOrders[0].OrderItems[0].Quantity);
        Assert.Equal(_expectApplicationDtoOrder.ShopOrders[0].OrderItems[0].Note, _actualApplicationDtoOrder.ShopOrders[0].OrderItems[0].Note);
#pragma warning disable CS8602
    }

    [Fact]
    [Trait("Category", "UnitTest")]
    public void MappingMssqlOrderAddOnsToApplicationOrderAddOnsDto()
    {
#pragma warning disable CS8602
        Assert.NotNull(_actualApplicationDtoOrder.ShopOrders[0].OrderItems[0].AddOns[0]);
        Assert.Equal(_expectApplicationDtoOrder.ShopOrders[0].OrderItems[0].AddOns[0].Id, _actualApplicationDtoOrder.ShopOrders[0].OrderItems[0].AddOns[0].Id);
        Assert.Equal(_expectApplicationDtoOrder.ShopOrders[0].OrderItems[0].AddOns[0].Name, _actualApplicationDtoOrder.ShopOrders[0].OrderItems[0].AddOns[0].Name);
        Assert.Equal(_expectApplicationDtoOrder.ShopOrders[0].OrderItems[0].AddOns[0].Price, _actualApplicationDtoOrder.ShopOrders[0].OrderItems[0].AddOns[0].Price);
        Assert.Equal(_expectApplicationDtoOrder.ShopOrders[0].OrderItems[0].AddOns[0].Quantity, _actualApplicationDtoOrder.ShopOrders[0].OrderItems[0].AddOns[0].Quantity);
#pragma warning disable CS8602
    }
}