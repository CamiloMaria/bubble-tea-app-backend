using BubbleTea.Domain.Entities;

namespace BubbleTea.Domain.Interfaces
{
    public interface IProductSizeRepository
    {
        Task<Response<IEnumerable<ProductSize>>> GetAllProductSizeAsync(int page, int pageSize);
        Task<Response<ProductSize>> GetProductSizeByIdAsync(int id);
        Task<Response<ProductSize>> CreateProductSizeAsync(ProductSize productSize);
        Task<Response<ProductSize>> UpdateProductSizeAsync(ProductSize productSize);
        Task<Response<ProductSize>> DeleteProductSizeAsync(int id);
        Task<Response<IEnumerable<ProductSize>>> GetProductSizeByProductIdAsync(int productId);
        Task<Response<IEnumerable<ProductSize>>> GetProductSizeBySizeIdAsync(int sizeId);
    }
}