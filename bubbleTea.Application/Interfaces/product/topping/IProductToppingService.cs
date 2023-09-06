using BubbleTea.Domain.Entities;

namespace BubbleTea.Application.Interfaces
{
    public interface IProductToppingService
    {
        Task<Response<IEnumerable<ProductTopping>>> GetAllProductTopping(int page, int pageSize);
        Task<Response<ProductTopping>> GetProductToppingById(int id);
        Task<Response<ProductTopping>> CreateProductTopping(ProductTopping topping);
        Task<Response<ProductTopping>> UpdateProductTopping(ProductTopping topping);
        Task<Response<ProductTopping>> DeleteProductTopping(int id);
        Task<Response<ProductTopping>> GetProductToppingByProductId(int productId);
        Task<Response<ProductTopping>> GetProductToppingByToppingId(int toppingId);
    }
}