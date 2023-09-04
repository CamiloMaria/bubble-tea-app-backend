using BubbleTea.Domain.Entities;

namespace BubbleTea.Domain.Interfaces
{
    public interface ICartRepository 
    {
        Task<Response<IEnumerable<Cart>>> GetAllCartAsync();
        Task<Response<Cart>> GetCartByIdAsync(int id);
        Task<Response<Cart>> CreateCartAsync(Cart cart);
        Task<Response<Cart>> UpdateCartAsync(Cart cart);
        Task<Response<Cart>> DeleteCartAsync(int id);
    }
}