
using AutoMapper;
using Infrastructure.Mapping;

namespace InfrastructureTest;

public class MappingOrderDtoToEntityTest
{
    private readonly Application.DTO.Order _applicationDtoOrder;
    private readonly Infrastructure.Mssql.Entity.Order _expectMssqlEntityOrder;
    private readonly Infrastructure.Mssql.Entity.Order _actualMssqlEntityOrder;
    private readonly IMapper _mapper;
    public MappingOrderDtoToEntityTest()
    {
        _applicationDtoOrder = new Application.DTO.Order
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
        _expectMssqlEntityOrder = new Infrastructure.Mssql.Entity.Order
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
        _mapper = new MapperConfiguration(cfg => { cfg.AddProfile<MappingOrderEntityToDto>(); }).CreateMapper();
        _actualMssqlEntityOrder = _mapper.Map<Infrastructure.Mssql.Entity.Order>(_applicationDtoOrder);
    }

    [Fact]
    [Trait("Category", "UnitTest")]
    public void MappingApplicationOrderDtoToMssqlOrderEntity()
    {
        Assert.Equal(_expectMssqlEntityOrder.Id, _actualMssqlEntityOrder.Id);
        Assert.Equal(_expectMssqlEntityOrder.CustomerID, _actualMssqlEntityOrder.CustomerID);
        Assert.Equal(_expectMssqlEntityOrder.DeliveryChannel, _actualMssqlEntityOrder.DeliveryChannel);
    }

    [Fact]
    [Trait("Category", "UnitTest")]
    public void MappingApplicationOrderDiscountDtoToMssqlOrderDiscountEntity()
    {
        Assert.NotNull(_actualMssqlEntityOrder.Discount);
        Assert.Equal(_expectMssqlEntityOrder.Discount!.Id, _actualMssqlEntityOrder.Discount!.Id);
        Assert.Equal(_expectMssqlEntityOrder.Discount.Code, _actualMssqlEntityOrder.Discount.Code);
        Assert.Equal(_expectMssqlEntityOrder.Discount.Description, _actualMssqlEntityOrder.Discount.Description);
        Assert.Equal(_expectMssqlEntityOrder.Discount.Operation, _actualMssqlEntityOrder.Discount.Operation);
        Assert.Equal(_expectMssqlEntityOrder.Discount.OperationAmount, _actualMssqlEntityOrder.Discount.OperationAmount);
        Assert.Equal(_expectMssqlEntityOrder.Discount.Condition, _actualMssqlEntityOrder.Discount.Condition);
        Assert.Equal(_expectMssqlEntityOrder.Discount.ConditionAmount, _actualMssqlEntityOrder.Discount.ConditionAmount);
        Assert.Equal(_expectMssqlEntityOrder.Discount.ExpireDate, _actualMssqlEntityOrder.Discount.ExpireDate);
    }

    [Fact]
    [Trait("Category", "UnitTest")]
    public void MappingApplicationOrderShopDtoToMssqlOrderShopEntity()
    {
#pragma warning disable CS8602
        Assert.NotNull(_actualMssqlEntityOrder.ShopOrders[0]);
        Assert.Equal(_expectMssqlEntityOrder.ShopOrders[0].Shop.Id, _actualMssqlEntityOrder.ShopOrders[0].Shop.Id);
        Assert.Equal(_expectMssqlEntityOrder.ShopOrders[0].Shop.Code, _actualMssqlEntityOrder.ShopOrders[0].Shop.Code);
        Assert.Equal(_expectMssqlEntityOrder.ShopOrders[0].Shop.Name, _actualMssqlEntityOrder.ShopOrders[0].Shop.Name);
        Assert.Equal(_expectMssqlEntityOrder.ShopOrders[0].Shop.Status, _actualMssqlEntityOrder.ShopOrders[0].Shop.Status);
#pragma warning disable CS8602
    }

    [Fact]
    [Trait("Category", "UnitTest")]
    public void MappingApplicationOrderListOrderItemDtoToMssqlOrderListOrderItemEntity()
    {
#pragma warning disable CS8602
        Assert.NotNull(_actualMssqlEntityOrder.ShopOrders[0].OrderItems[0]);
        Assert.Equal(_expectMssqlEntityOrder.ShopOrders[0].OrderItems[0].Id, _actualMssqlEntityOrder.ShopOrders[0].OrderItems[0].Id);
        Assert.Equal(_expectMssqlEntityOrder.ShopOrders[0].OrderItems[0].Name, _actualMssqlEntityOrder.ShopOrders[0].OrderItems[0].Name);
        Assert.Equal(_expectMssqlEntityOrder.ShopOrders[0].OrderItems[0].Price, _actualMssqlEntityOrder.ShopOrders[0].OrderItems[0].Price);
        Assert.Equal(_expectMssqlEntityOrder.ShopOrders[0].OrderItems[0].Quantity, _actualMssqlEntityOrder.ShopOrders[0].OrderItems[0].Quantity);
        Assert.Equal(_expectMssqlEntityOrder.ShopOrders[0].OrderItems[0].Note, _actualMssqlEntityOrder.ShopOrders[0].OrderItems[0].Note);
#pragma warning disable CS8602
    }

    [Fact]
    [Trait("Category", "UnitTest")]
    public void MappingApplicationOrderAddOnsDtoToMssqlOrderAddOns()
    {
#pragma warning disable CS8602
        Assert.NotNull(_actualMssqlEntityOrder.ShopOrders[0].OrderItems[0].AddOns[0]);
        Assert.Equal(_expectMssqlEntityOrder.ShopOrders[0].OrderItems[0].AddOns[0].Id, _actualMssqlEntityOrder.ShopOrders[0].OrderItems[0].AddOns[0].Id);
        Assert.Equal(_expectMssqlEntityOrder.ShopOrders[0].OrderItems[0].AddOns[0].Name, _actualMssqlEntityOrder.ShopOrders[0].OrderItems[0].AddOns[0].Name);
        Assert.Equal(_expectMssqlEntityOrder.ShopOrders[0].OrderItems[0].AddOns[0].Price, _actualMssqlEntityOrder.ShopOrders[0].OrderItems[0].AddOns[0].Price);
        Assert.Equal(_expectMssqlEntityOrder.ShopOrders[0].OrderItems[0].AddOns[0].Quantity, _actualMssqlEntityOrder.ShopOrders[0].OrderItems[0].AddOns[0].Quantity);
#pragma warning disable CS8602
    }
}