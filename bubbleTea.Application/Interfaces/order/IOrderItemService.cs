using BubbleTea.Domain.Entities;

namespace BubbleTea.Application.Interfaces
{
    public interface IOrderItemService
    {
        Task<Response<IEnumerable<OrderItem>>> GetAllOrderItem(int page, int pageSize);
        Task<Response<OrderItem>> GetOrderItemById(int orderId);
        Task<Response<OrderItem>> CreateOrderItem(OrderItem orderItem);
        Task<Response<OrderItem>> UpdateOrderItem(OrderItem orderItem);
        Task<Response<OrderItem>> DeleteOrderItem(int id);
        Task<Response<IEnumerable<OrderItem>>> GetOrderItemByOrderId(int orderId);
        Task<Response<IEnumerable<OrderItem>>> GetOrderItemByProductId(int productId);
    }
}
