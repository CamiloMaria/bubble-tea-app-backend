using BubbleTea.Domain.Entities;

namespace BubbleTea.Domain.Interfaces
{
    public interface IProductSize 
    {
        Task<Response<IEnumerable<ProductSize>>> GetAllProductSizAsync();
        Task<Response<ProductSize>> GetProductSizeByIdAsync(int id);
        Task<Response<ProductSize>> CreateProductSizeAsync(ProductSize productSize);
        Task<Response<ProductSize>> UpdateProductSizeAsync(ProductSize productSize);
        Task<Response<ProductSize>> DeleteProductSizeAsync(int id);
    }
}