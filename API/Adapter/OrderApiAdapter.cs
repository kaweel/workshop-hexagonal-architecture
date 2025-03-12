using Application.Port;
using Application.DTO;

namespace Controller.Adapter;

public class OrderApiAdapter : IOrderServicePort
{
    private readonly IOrderService _orderService;

    public OrderApiAdapter(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task<Order?> GetOrderById(int id)
    {
        return await _orderService.GetOrderById(id);
    }
}
