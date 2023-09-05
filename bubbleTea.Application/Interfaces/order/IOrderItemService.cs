using BubbleTea.Domain.Entities;

namespace BubbleTea.Application.Interfaces
{
    public interface IOrderItemService
    {
        Task<Response<IEnumerable<OrderItem>>> GetAllOrderItem();
        Task<Response<IEnumerable<OrderItem>>> GetOrderItemById(int orderId);
        Task<Response<OrderItem>> AddOrderItem(OrderItem orderItem);
        Task<Response<OrderItem>> UpdateOrderItem(OrderItem orderItem);
        Task<Response<OrderItem>> DeleteOrderItem(int id);
    }
}
