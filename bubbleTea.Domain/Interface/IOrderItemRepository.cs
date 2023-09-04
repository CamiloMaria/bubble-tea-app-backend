using BubbleTea.Domain.Entities;

namespace BubbleTea.Domain.Interfaces
{
    public interface IOrderItemRepository
    {
        Task<Response<IEnumerable<OrderItem>>> GetByOrderIdAsync(int orderId);
        Task<Response<OrderItem>> AddOrderItemAsync(OrderItem orderItem);
        Task<Response<OrderItem>> UpdateOrderItemAsync(OrderItem orderItem);
        Task<Response<OrderItem>> DeleteOrderItemAsync(int id);
    }
}
