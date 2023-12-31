using BubbleTea.Domain.Entities;

namespace BubbleTea.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<Response<IEnumerable<Order>>> GetAllOrderAsync(int page, int pageSize);
        Task<Response<Order>> GetOrderByIdAsync(int id);
        Task<Response<Order>> CreateOrderAsync(Order order);
        Task<Response<Order>> UpdateOrderAsync(Order order);
        Task<Response<Order>> DeleteOrderAsync(int id);
        Task<Response<IEnumerable<Order>>> GetOrderByUserIdAsync(int userId);
    }
}