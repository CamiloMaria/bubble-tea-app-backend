using BubbleTea.Domain.Entities;

namespace BubbleTea.Application.Interfaces
{
    public interface IProductSizeService
    {
        Task<Response<IEnumerable<ProductSize>>> GetAllProductSize(int page, int pageSize);
        Task<Response<ProductSize>> GetProductSizeById(int id);
        Task<Response<ProductSize>> CreateProductSize(ProductSize productSize);
        Task<Response<ProductSize>> UpdateProductSize(ProductSize productSize);
        Task<Response<ProductSize>> DeleteProductSize(int id);
        Task<Response<IEnumerable<ProductSize>>> GetProductSizeByProductId(int productId);
        Task<Response<IEnumerable<ProductSize>>> GetProductSizeBySizeId(int sizeId);
    }
}