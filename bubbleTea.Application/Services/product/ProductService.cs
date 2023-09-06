using BubbleTea.Domain.Entities;
using BubbleTea.Domain.Interfaces;
using BubbleTea.Application.Interfaces;

namespace BubbleTea.Application.Services
{
    public class ProductService: IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }

        public async Task<Response<IEnumerable<Product>>> GetAllProduct(int page, int pageSize)
        {
            return await _productRepository.GetAllProductAsync(page, pageSize);
        }

        public async Task<Response<Product>> GetProductById(int id)
        {
            return await _productRepository.GetProductByIdAsync(id);
        }

        public async Task<Response<Product>> CreateProduct(Product product)
        {
            return await _productRepository.CreateProductAsync(product);
        }

        public async Task<Response<Product>> UpdateProduct(Product product)
        {
            return await _productRepository.UpdateProductAsync(product);
        }

        public async Task<Response<Product>> DeleteProduct(int id)
        {
            return await _productRepository.DeleteProductAsync(id);
        }

        public async Task<Response<IEnumerable<Product>>> GetProductByCategory(int categoryId, int page, int pageSize)
        {
            return await _productRepository.GetProductByCategoryAsync(categoryId, page, pageSize);
        }
    }
}