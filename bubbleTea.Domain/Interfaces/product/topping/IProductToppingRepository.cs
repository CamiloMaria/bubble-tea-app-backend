using BubbleTea.Domain.Entities;

namespace BubbleTea.Domain.Interfaces
{
    public interface IProductToppingRepository
    {
        Task<Response<IEnumerable<ProductTopping>>> GetAllProductToppingAsync(int page, int pageSize);
        Task<Response<ProductTopping>> GetProductToppingByIdAsync(int id);
        Task<Response<ProductTopping>> CreateProductToppingAsync(ProductTopping topping);
        Task<Response<ProductTopping>> UpdateProductToppingAsync(ProductTopping topping);
        Task<Response<ProductTopping>> DeleteProductToppingAsync(int id);
        Task<Response<ProductTopping>> GetProductToppingByProductIdAsync(int productId);
        Task<Response<ProductTopping>> GetProductToppingByToppingIdAsync(int toppingId);
    }
}