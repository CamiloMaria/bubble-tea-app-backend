using BubbleTea.Domain.Entities;
using BubbleTea.Domain.Interfaces;
using BubbleTea.Application.Interfaces;

namespace BubbleTea.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            this._orderRepository = orderRepository;
        }

        public async Task<Response<IEnumerable<Order>>> GetAllOrder(int page, int pageSize)
        {
            return await _orderRepository.GetAllOrderAsync(page, pageSize);
        }

        public async Task<Response<Order>> GetOrderById(int id)
        {
            return await _orderRepository.GetOrderByIdAsync(id);
        }

        public async Task<Response<Order>> CreateOrder(Order order)
        {
            return await _orderRepository.CreateOrderAsync(order);
        }

        public async Task<Response<Order>> UpdateOrder(Order order)
        {
            return await _orderRepository.UpdateOrderAsync(order);
        }

        public async Task<Response<Order>> DeleteOrder(int id)
        {
            return await _orderRepository.DeleteOrderAsync(id);
        }

        public async Task<Response<IEnumerable<Order>>> GetOrderByUserId(int userId)
        {
            return await _orderRepository.GetOrderByUserIdAsync(userId);
        }
    }
}