using Application.DTO;

namespace Application.Port;

public interface IOrderRepositoryPort
{
    Task<Order?> GetByIdAsync(int id);
    Task<Order?> Upsert(Order order);
    Task<List<Order>?> Upserts(List<Order> order);
    Task<bool> DeleteByIdAsync(int id);
    Task<bool> DeleteByIdsAsync(List<int> id);
}
