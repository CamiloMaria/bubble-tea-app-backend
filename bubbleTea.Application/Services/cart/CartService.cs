using BubbleTea.Domain.Entities;
using BubbleTea.Domain.Interfaces;
using BubbleTea.Application.Interfaces;

namespace BubbleTea.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            this._cartRepository = cartRepository;
        }

        public async Task<Response<IEnumerable<Cart>>> GetAllCart(int page, int pageSize)
        {
            return await _cartRepository.GetAllCartAsync(page, pageSize);
        }

        public async Task<Response<Cart>> GetCartById(int id)
        {
            return await _cartRepository.GetCartByIdAsync(id);
        }

        public async Task<Response<Cart>> CreateCart(Cart cart)
        {
            return await _cartRepository.CreateCartAsync(cart);
        }

        public async Task<Response<Cart>> UpdateCart(Cart cart)
        {
            return await _cartRepository.UpdateCartAsync(cart);
        }

        public async Task<Response<Cart>> DeleteCart(int id)
        {
            return await _cartRepository.DeleteCartAsync(id);
        }
    }
}