using Application.DTO;

namespace Application.Port;

public interface IOrderService
{
    Task<Order?> GetOrderById(int id);
}
