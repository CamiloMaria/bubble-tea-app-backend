using BubbleTea.Domain.Entities;

namespace BubbleTea.Application.Interfaces
{
    public interface IOrderService
    {
        Task<Response<IEnumerable<Order>>> GetAllOrder(int page, int pageSize);
        Task<Response<Order>> GetOrderById(int id);
        Task<Response<Order>> CreateOrder(Order order);
        Task<Response<Order>> UpdateOrder(Order order);
        Task<Response<Order>> DeleteOrder(int id);
        Task<Response<IEnumerable<Order>>> GetOrderByUserId(int userId);
    }
}