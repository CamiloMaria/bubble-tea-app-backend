using BubbleTea.Domain.Entities;

namespace BubbleTea.Application.Interfaces
{
    public interface IProductToppingService
    {
        Task<Response<IEnumerable<Topping>>> GetAllTopping();
        Task<Response<Topping>> GetToppingById(int id);
        Task<Response<Topping>> AddTopping(Topping topping);
        Task<Response<Topping>> UpdateTopping(Topping topping);
        Task<Response<Topping>> DeleteTopping(int id);
    }
}