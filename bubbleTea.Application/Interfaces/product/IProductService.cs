using BubbleTea.Domain.Entities;

namespace BubbleTea.Application.Interfaces
{
    public interface IProductService
    {
        Task<Response<IEnumerable<Product>>> GetAllProduct(int page, int pageSize);
        Task<Response<Product>> GetProductById(int id);
        Task<Response<Product>> CreateProduct(Product product);
        Task<Response<Product>> UpdateProduct(Product product);
        Task<Response<Product>> DeleteProduct(int id);
    }
}