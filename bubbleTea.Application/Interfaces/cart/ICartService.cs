using BubbleTea.Domain.Entities;

namespace BubbleTea.Application.Interfaces
{
    public interface ICartService
    {
        Task<Response<IEnumerable<Cart>>> GetAllCart(int page, int pageSize);
        Task<Response<Cart>> GetCartById(int id);
        Task<Response<Cart>> CreateCart(Cart cart);
        Task<Response<Cart>> UpdateCart(Cart cart);
        Task<Response<Cart>> DeleteCart(int id);
    }
}