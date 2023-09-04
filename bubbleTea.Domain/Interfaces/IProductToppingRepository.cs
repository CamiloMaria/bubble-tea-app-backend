using BubbleTea.Domain.Entities;

namespace BubbleTea.Domain.Interfaces
{
    public interface IProductToppingRepository
    {
        Task<Response<IEnumerable<Topping>>> GetAllToppingAsync();
        Task<Response<Topping>> GetToppingByIdAsync(int id);
        Task<Response<Topping>> AddToppingAsync(Topping topping);
        Task<Response<Topping>> UpdateToppingAsync(Topping topping);
        Task<Response<Topping>> DeleteToppingAsync(int id);
    }
}