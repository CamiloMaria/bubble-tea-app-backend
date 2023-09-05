using BubbleTea.Domain.Entities;

namespace BubbleTea.Application.Interfaces
{
    public interface IOrderService
    {
        Task<Response<IEnumerable<Order>>> GetAllOrder();
        Task<Response<Order>> GetORderById(int id);
        Task<Response<Order>> AddOrder(Order order);
        Task<Response<Order>> UpdateOrder(Order order);
        Task<Response<Order>> DeleteOrder(int id);
        Task<Response<IEnumerable<Order>>> GetOrderByUserId(int userId);
    }
}