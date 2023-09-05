using BubbleTea.Domain.Entities;
using BubbleTea.Domain.Interfaces;
using BubbleTea.Application.Interfaces;

namespace BubbleTea.Application.Services
{
    public class ProductSizeService : IProductSizeService
    {
        private readonly IProductSizeRepository _productSizeRepository;

        public ProductSizeService(IProductSizeRepository productSizeRepository)
        {
            this._productSizeRepository = productSizeRepository;
        }

        public async Task<Response<IEnumerable<ProductSize>>> GetAllProductSize(int page, int pageSize)
        {
            return await _productSizeRepository.GetAllProductSizeAsync(page, pageSize);
        }

        public async Task<Response<ProductSize>> GetProductSizeById(int id)
        {
            return await _productSizeRepository.GetProductSizeByIdAsync(id);
        }

        public async Task<Response<ProductSize>> CreateProductSize(ProductSize productSize)
        {
            return await _productSizeRepository.CreateProductSizeAsync(productSize);
        }

        public async Task<Response<ProductSize>> UpdateProductSize(ProductSize productSize)
        {
            return await _productSizeRepository.UpdateProductSizeAsync(productSize);
        }

        public async Task<Response<ProductSize>> DeleteProductSize(int id)
        {
            return await _productSizeRepository.DeleteProductSizeAsync(id);
        }

        public async Task<Response<IEnumerable<ProductSize>>> GetProductSizeByProductId(int productId)
        {
            return await _productSizeRepository.GetProductSizeByProductIdAsync(productId);
        }

        public async Task<Response<IEnumerable<ProductSize>>> GetProductSizeBySizeId(int sizeId)
        {
            return await _productSizeRepository.GetProductSizeBySizeIdAsync(sizeId);
        }
    }
}