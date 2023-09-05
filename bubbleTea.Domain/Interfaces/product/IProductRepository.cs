using BubbleTea.Domain.Entities;

namespace BubbleTea.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<Response<IEnumerable<Product>>> GetAllProductAsync();
        Task<Response<Product>> GetProductByIdAsync(int id);
        Task<Response<Product>> AddProductAsync(Product product);
        Task<Response<Product>> UpdatProductAsync(Product product);
        Task<Response<Product>> DeleteProductAsync(int id);
    }
}