using BubbleTea.Domain.Entities;
using BubbleTea.Domain.Interfaces;
using BubbleTea.Application.Interfaces;

namespace BubbleTea.Application.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;

        public OrderItemService(IOrderItemRepository orderItemRepository)
        {
            this._orderItemRepository = orderItemRepository;
        }

        public async Task<Response<IEnumerable<OrderItem>>> GetAllOrderItem(int page, int pageSize)
        {
            return await _orderItemRepository.GetAllOrderItemAsync(page, pageSize);
        }

        public async Task<Response<OrderItem>> GetOrderItemById(int id)
        {
            return await _orderItemRepository.GetOrderItemByIdAsync(id);
        }

        public async Task<Response<OrderItem>> CreateOrderItem(OrderItem orderItem)
        {
            return await _orderItemRepository.CreateOrderItemAsync(orderItem);
        }

        public async Task<Response<OrderItem>> UpdateOrderItem(OrderItem orderItem)
        {
            return await _orderItemRepository.UpdateOrderItemAsync(orderItem);
        }

        public async Task<Response<OrderItem>> DeleteOrderItem(int id)
        {
            return await _orderItemRepository.DeleteOrderItemAsync(id);
        }

        public async Task<Response<OrderItem>> GetOrderItemByOrderId(int orderId)
        {
            return await _orderItemRepository.GetOrderItemByOrderIdAsync(orderId);
        }

        public async Task<Response<OrderItem>> GetOrderItemByProductId(int productId)
        {
            return await _orderItemRepository.GetOrderItemByProductIdAsync(productId);
        }
    }
}