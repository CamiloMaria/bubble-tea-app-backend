using BubbleTea.Domain.Entities;

namespace BubbleTea.Domain.Interfaces
{
    public interface IProductToppingRepository
    {
        Task<Response<IEnumerable<Topping>>> GetAllProductToppingAsync(int page, int pageSize);
        Task<Response<Topping>> GetProductToppingByIdAsync(int id);
        Task<Response<Topping>> CreateProductToppingAsync(Topping topping);
        Task<Response<Topping>> UpdateProductToppingAsync(Topping topping);
        Task<Response<Topping>> DeleteProductToppingAsync(int id);
        Task<Response<IEnumerable<Topping>>> GetProductToppingByProductIdAsync(int productId);
    }
}