using BubbleTea.Domain.Entities;

namespace BubbleTea.Domain.Interfaces
{
    public interface IOrderItemRepository
    {
        Task<Response<IEnumerable<OrderItem>>> GetAllOrderItemAsync(int page, int pageSize);
        Task<Response<OrderItem>> GetOrderItemByIdAsync(int id);
        Task<Response<OrderItem>> CreateOrderItemAsync(OrderItem orderItem);
        Task<Response<OrderItem>> UpdateOrderItemAsync(OrderItem orderItem);
        Task<Response<OrderItem>> DeleteOrderItemAsync(int id);
        Task<Response<OrderItem>> GetOrderItemByOrderIdAsync(int orderId);
        Task<Response<OrderItem>> GetOrderItemByProductIdAsync(int productId);
    }
}
