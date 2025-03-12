using Application.Port;
using AutoMapper;
using Infrastructure.Mapping;
using Infrastructure.Mssql;
using InfrastructureTest;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Test.Order;

public class GetOrderByIdTest : IAsyncLifetime
{
#pragma warning disable CS8618
    private MssqlTestContainer _mssqlTestContainer;
    private MssqlDbContext _dbContext;
    private IOrderRepositoryPort _orderRepositoryPort;
    private IMapper _mapper;
#pragma warning disable CS8618

    public async Task InitializeAsync()
    {
        _mssqlTestContainer = new MssqlTestContainer();
        await _mssqlTestContainer.StartAsync();

        _dbContext = new MssqlDbContext(
            new DbContextOptionsBuilder<MssqlDbContext>()
            .UseSqlServer(_mssqlTestContainer.GetConnectionString())
            .UseLazyLoadingProxies()
            .LogTo(Console.WriteLine, LogLevel.Information)
            .Options
        );
        await _dbContext.Database.MigrateAsync();

        var order = new Mssql.Entity.Order
        {
            CustomerID = 1,
            DeliveryChannel = "delivery",
            Discount = new Mssql.Entity.Discount
            {
                Code = "JD20",
                Description = "ใจดี 20%",
                Operation = "%",
                OperationAmount = new decimal(0.1),
                Condition = ">",
                ConditionAmount = 100,
                ExpireDate = new DateTime(2025, 2, 16, 0, 0, 0, DateTimeKind.Utc),
            },
            ShopOrders = new List<Mssql.Entity.ShopOrder> {
                new Mssql.Entity.ShopOrder {
                    Shop = new Mssql.Entity.Shop{
                        Code = "001",
                        Name = ".NET Thailand",
                        Status = "Active",
                    },
                    OrderItems = new List<Mssql.Entity.OrderItem> {
                        new Mssql.Entity.OrderItem {
                            Name = "Milk Tea",
                            Price = 89.00m,
                            Quantity = 1,
                            AddOns = new List<Mssql.Entity.AddOn> {
                                new Mssql.Entity.AddOn {
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
        await _dbContext.Orders.AddAsync(order);
        await _dbContext.SaveChangesAsync();

        _mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingOrderEntityToDto>();
        }).CreateMapper();
        _orderRepositoryPort = new OrderMssqlAdapter(_dbContext, _mapper);
    }

    public async Task DisposeAsync()
    {
        await _dbContext.Database.EnsureDeletedAsync();
        await _mssqlTestContainer.StopAsync();
    }

    [Fact]
    [Trait("Category", "IntegrationTest")]
    public async Task GetOrderById_WhenOrderNotFound_ShouldReturnNull()
    {
        var actual = await _orderRepositoryPort.GetByIdAsync(2);

        Assert.Null(actual);
    }

    [Fact]
    [Trait("Category", "IntegrationTest")]
    public async Task GetOrderById_WhenOrderFound_ShouldReturnOrder()
    {
        var actual = await _orderRepositoryPort.GetByIdAsync(1);

        Assert.NotNull(actual);
    }

    [Fact]
    [Trait("Category", "IntegrationTest")]
    public async Task DeleteOrderByIdFail_WhenOrderNotFound_ShouldReturnTrue()
    {
        var actual = await _orderRepositoryPort.DeleteByIdAsync(2);

        Assert.False(actual);
    }

    [Fact]
    [Trait("Category", "IntegrationTest")]
    public async Task DeleteOrderByIdSuccess_WhenOrderFound_ShouldReturnTrue()
    {
        var actual = await _orderRepositoryPort.DeleteByIdAsync(1);

        Assert.True(actual);
    }

    [Fact]
    [Trait("Category", "IntegrationTest")]
    public async Task DeleteOrderByIdsFail_WhenOrderNotFound_ShouldReturnTrue()
    {
        var actual = await _orderRepositoryPort.DeleteByIdsAsync(new List<int> { 2, 3 });

        Assert.False(actual);
    }

    [Fact]
    [Trait("Category", "IntegrationTest")]
    public async Task DeleteOrderByIdsSuccess_WhenOrderFound_ShouldReturnTrue()
    {
        var actual = await _orderRepositoryPort.DeleteByIdsAsync(new List<int> { 1 });

        Assert.True(actual);
    }

    [Fact]
    [Trait("Category", "IntegrationTest")]
    public async Task UpsertSuccess_WhenOrderIsCreate_ShouldReturnCreatedOrder()
    {
        var before = await _dbContext.Orders.AsNoTracking().CountAsync();
        var dto = new Application.DTO.Order
        {
            CustomerID = 1,
            DeliveryChannel = "delivery",
            Discount = new Application.DTO.Discount
            {
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
                        Code = "001",
                        Name = ".NETX Thailand",
                        Status = "Active",
                    },
                    OrderItems = new List<Application.DTO.OrderItem> {
                        new Application.DTO.OrderItem {
                            Name = "Milk Coffeee",
                            Price = 89.00m,
                            Quantity = 1,
                            AddOns = new List<Application.DTO.AddOn> {
                                new Application.DTO.AddOn {
                                    Name = "Singature Coffeee",
                                    Price = new decimal(20.00),
                                    Quantity = 1
                                }
                            }
                        }
                    }
                }
            }
        };

        var _ = await _orderRepositoryPort.Upsert(dto);

        var after = await _dbContext.Orders.AsNoTracking().CountAsync();
        Assert.Equal(before + 1, after);
    }

    [Fact]
    [Trait("Category", "IntegrationTest")]
    public async Task UpsertSuccess_WhenOrderIsUpdate_ShouldReturnUpdatedOrder()
    {
        var entity = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == 1);
        entity!.DeliveryChannel = "selfpickup";
        var dto = _mapper.Map<Application.DTO.Order>(entity);

        var actual = await _orderRepositoryPort.Upsert(dto);

        Assert.Equal("selfpickup", actual!.DeliveryChannel);
    }

    [Fact]
    [Trait("Category", "IntegrationTest")]
    public async Task UpsertsSuccess_WhenOrderIsCreate_ShouldReturnCreatedOrder()
    {
        var before = await _dbContext.Orders.AsNoTracking().CountAsync();
        var dto = new Application.DTO.Order
        {
            CustomerID = 1,
            DeliveryChannel = "delivery",
            Discount = new Application.DTO.Discount
            {
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
                        Code = "001",
                        Name = ".NETX Thailand",
                        Status = "Active",
                    },
                    OrderItems = new List<Application.DTO.OrderItem> {
                        new Application.DTO.OrderItem {
                            Name = "Milk Coffeee",
                            Price = 89.00m,
                            Quantity = 1,
                            AddOns = new List<Application.DTO.AddOn> {
                                new Application.DTO.AddOn {
                                    Name = "Singature Coffeee",
                                    Price = new decimal(20.00),
                                    Quantity = 1
                                }
                            }
                        }
                    }
                }
            }
        };
        var dto1 = new Application.DTO.Order
        {
            CustomerID = 2,
            DeliveryChannel = "delivery",
            Discount = new Application.DTO.Discount
            {
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
                        Code = "001",
                        Name = ".NETX Thailand",
                        Status = "Active",
                    },
                    OrderItems = new List<Application.DTO.OrderItem> {
                        new Application.DTO.OrderItem {
                            Name = "Milk Coffeee",
                            Price = 89.00m,
                            Quantity = 1,
                            AddOns = new List<Application.DTO.AddOn> {
                                new Application.DTO.AddOn {
                                    Name = "Singature Coffeee",
                                    Price = new decimal(20.00),
                                    Quantity = 1
                                }
                            }
                        }
                    }
                }
            }
        };
        var dtos = new List<Application.DTO.Order> { dto, dto1 };

        var _ = await _orderRepositoryPort.Upserts(dtos);

        var after = await _dbContext.Orders.AsNoTracking().CountAsync();
        Assert.Equal(before + 2, after);
    }

    [Fact]
    [Trait("Category", "IntegrationTest")]
    public async Task UpsertsSuccess_WhenOrderIsUpdate_ShouldReturnUpdatedOrder()
    {
        var before = await _dbContext.Orders.AsNoTracking().CountAsync();
        var entity = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == 1);
        entity!.DeliveryChannel = "selfpickup";
        var dtos = new List<Application.DTO.Order> { _mapper.Map<Application.DTO.Order>(entity) };

        var _ = await _orderRepositoryPort.Upserts(dtos);

        var after = await _dbContext.Orders.AsNoTracking().CountAsync();

        Assert.Equal(before, after);
    }

}
