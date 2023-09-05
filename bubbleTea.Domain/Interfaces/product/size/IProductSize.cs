using BubbleTea.Domain.Entities;

namespace BubbleTea.Domain.Interfaces
{
    public interface IProductSizeRepository
    {
        Task<Response<IEnumerable<ProductSize>>> GetAllProductSizeAsync();
        Task<Response<ProductSize>> GetProductSizeByIdAsync(int id);
        Task<Response<ProductSize>> CreateProductSizeAsync(ProductSize productSize);
        Task<Response<ProductSize>> UpdateProductSizeAsync(ProductSize productSize);
        Task<Response<ProductSize>> DeleteProductSizeAsync(int id);
    }
}