using BubbleTea.Domain.Entities;

namespace BubbleTea.Application.Interfaces
{
    public interface IToppingService
    {
        Task<Response<IEnumerable<Topping>>> GetAllTopping(int page, int pageSize);
        Task<Response<Topping>> GetToppingById(int id);
        Task<Response<Topping>> CreateTopping(Topping topping);
        Task<Response<Topping>> UpdateTopping(Topping topping);
        Task<Response<Topping>> DeleteTopping(int id);
    }
}