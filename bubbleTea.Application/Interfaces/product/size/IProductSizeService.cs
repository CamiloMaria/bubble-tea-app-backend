using BubbleTea.Domain.Entities;

namespace BubbleTea.Application.Interfaces
{
    public interface IProductSizeService
    {
        Task<Response<IEnumerable<ProductSize>>> GetAllProductSize();
        Task<Response<ProductSize>> GetProductSizeByIde(int id);
        Task<Response<ProductSize>> CreateProductSizee(ProductSize productSize);
        Task<Response<ProductSize>> UpdateProductSizee(ProductSize productSize);
        Task<Response<ProductSize>> DeleteProductSizee(int id);
    }
}