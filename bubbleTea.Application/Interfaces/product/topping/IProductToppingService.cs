using BubbleTea.Domain.Entities;

namespace BubbleTea.Application.Interfaces
{
    public interface IProductToppingService
    {
        Task<Response<IEnumerable<Topping>>> GetAllProductTopping(int page, int pageSize);
        Task<Response<Topping>> GetProductToppingById(int id);
        Task<Response<Topping>> CreateProductTopping(Topping topping);
        Task<Response<Topping>> UpdateProductTopping(Topping topping);
        Task<Response<Topping>> DeleteProductTopping(int id);
        Task<Response<IEnumerable<Topping>>> GetProductToppingByProductId(int productId);
    }
}