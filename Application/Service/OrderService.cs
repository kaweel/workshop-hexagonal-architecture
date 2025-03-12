using Application.Port;
using Common.Provider;
using Common.ApiException;
using Application.DTO;
using AutoMapper;

namespace Application.Service;

public class OrderService : IOrderService
{
    private readonly IOrderRepositoryPort _orderRepositoryPort;
    private readonly IMapper _mapper;
    private readonly IDateTimeProvider _dateTimeProvider;

    public OrderService(IOrderRepositoryPort orderRepositoryPort, IMapper mapper, IDateTimeProvider dateTimeProvider)
    {
        _orderRepositoryPort = orderRepositoryPort;
        _mapper = mapper;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Order?> GetOrderById(int id)
    {
        var dto = await _orderRepositoryPort.GetByIdAsync(id);
        if (dto == null)
        {
            throw new NotFoundException("order not found");
        }
        return _mapper.Map<Order>(_mapper.Map<Domain.Entity.Order>(dto));
    }
}
