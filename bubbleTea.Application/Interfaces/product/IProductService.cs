using BubbleTea.Domain.Entities;

namespace BubbleTea.Application.Interfaces
{
    public interface IProductService
    {
        Task<Response<IEnumerable<Product>>> GetAllProduct();
        Task<Response<Product>> GetProductById(int id);
        Task<Response<Product>> AddProduct(Product product);
        Task<Response<Product>> UpdatProduct(Product product);
        Task<Response<Product>> DeleteProduct(int id);
    }
}