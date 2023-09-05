using BubbleTea.Domain.Entities;

namespace BubbleTea.Domain.Interfaces
{
    public interface IToppingRepository
    {
        Task<Response<Topping>> GetToppingByIdAsync(int id);
        Task<Response<IEnumerable<Topping>>> GetAllToppingAsync(int page, int pageSize);
        Task<Response<Topping>> CreateToppingAsync(Topping topping);
        Task<Response<Topping>> UpdateToppingAsync(Topping topping);
        Task<Response<Topping>> DeleteToppingAsync(int id);
    }
}