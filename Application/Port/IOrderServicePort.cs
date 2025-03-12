using Application.DTO;

namespace Application.Port;

public interface IOrderServicePort
{
    Task<Order?> GetOrderById(int id);
}
