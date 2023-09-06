using BubbleTea.Domain.Entities;

namespace BubbleTea.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<Response<IEnumerable<Product>>> GetAllProductAsync(int page, int pageSize);
        Task<Response<Product>> GetProductByIdAsync(int id);
        Task<Response<Product>> CreateProductAsync(Product product);
        Task<Response<Product>> UpdateProductAsync(Product product);
        Task<Response<Product>> DeleteProductAsync(int id);
        Task<Response<IEnumerable<Product>>> GetProductByCategoryAsync(int categoryId, int page, int pageSize);
    }
}