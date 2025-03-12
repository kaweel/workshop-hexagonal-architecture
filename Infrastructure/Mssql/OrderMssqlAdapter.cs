using AutoMapper;
using Application.Port;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Mssql;

public class OrderMssqlAdapter : IOrderRepositoryPort
{
    private readonly MssqlDbContext _mssqlDbContext;
    private readonly IMapper _mapper;

    public OrderMssqlAdapter(MssqlDbContext mssqlDbContext, IMapper mapper)
    {
        _mssqlDbContext = mssqlDbContext;
        _mapper = mapper;
    }

    public async Task<Application.DTO.Order?> GetByIdAsync(int id)
    {
        var order = await _mssqlDbContext.Orders
        .Include(o => o.Discount)
        .Include(o => o.ShopOrders)
            .ThenInclude(o => o.Shop)
        .Include(o => o.ShopOrders)
            .ThenInclude(o => o.OrderItems)
                .ThenInclude(o => o.AddOns)
        .FirstOrDefaultAsync(o => o.Id == id);
        if (order == null)
        {
            return null;
        }
        // Console.WriteLine($"ABC : {order.DeliveryChannel}");
        return _mapper.Map<Application.DTO.Order>(order);
    }

    public async Task<Application.DTO.Order?> Upsert(Application.DTO.Order dto)
    {
        var entity = _mapper.Map<Entity.Order>(dto);
        if (entity.Id == 0)
        {
            await _mssqlDbContext.Orders.AddAsync(entity);
        }
        else
        {
            _mssqlDbContext.ChangeTracker.Clear();
            _mssqlDbContext.Orders.Update(entity);
        }
        await _mssqlDbContext.SaveChangesAsync();
        return _mapper.Map<Application.DTO.Order>(entity);
    }

    public async Task<List<Application.DTO.Order>?> Upserts(List<Application.DTO.Order> dtos)
    {
        var entities = _mapper.Map<List<Entity.Order>>(dtos);
        var toInsert = entities.Where(e => e.Id == 0).ToList();
        if (toInsert.Any())
        {
            await _mssqlDbContext.Orders.AddRangeAsync(toInsert);
        }
        var toUpdate = entities.Where(e => e.Id != 0).ToList();
        if (toUpdate.Any())
        {
            _mssqlDbContext.ChangeTracker.Clear();
            _mssqlDbContext.Orders.UpdateRange(toUpdate);
        }
        await _mssqlDbContext.SaveChangesAsync();
        return _mapper.Map<List<Application.DTO.Order>>(entities);
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        return await _mssqlDbContext.Orders.Where(o => o.Id == id).ExecuteDeleteAsync() > 0;
    }

    public async Task<bool> DeleteByIdsAsync(List<int> ids)
    {
        return await _mssqlDbContext.Orders.Where(o => ids.Contains(o.Id)).ExecuteDeleteAsync() > 0;
    }
}