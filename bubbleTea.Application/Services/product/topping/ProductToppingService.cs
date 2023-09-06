using BubbleTea.Domain.Entities;
using BubbleTea.Domain.Interfaces;
using BubbleTea.Application.Interfaces;

namespace BubbleTea.Application.Services
{
    public class ProductToppingService : IProductToppingService
    {
        private readonly IProductToppingRepository _productToppingRepository;

        public ProductToppingService(IProductToppingRepository productToppingRepository)
        {
            this._productToppingRepository = productToppingRepository;
        }

        public async Task<Response<IEnumerable<ProductTopping>>> GetAllProductTopping(int page, int pageSize)
        {
            return await _productToppingRepository.GetAllProductToppingAsync(page, pageSize);
        }

        public async Task<Response<ProductTopping>> GetProductToppingById(int id)
        {
            return await _productToppingRepository.GetProductToppingByIdAsync(id);
        }

        public async Task<Response<ProductTopping>> CreateProductTopping(ProductTopping productTopping)
        {
            return await _productToppingRepository.CreateProductToppingAsync(productTopping);
        }

        public async Task<Response<ProductTopping>> UpdateProductTopping(ProductTopping productTopping)
        {
            return await _productToppingRepository.UpdateProductToppingAsync(productTopping);
        }

        public async Task<Response<ProductTopping>> DeleteProductTopping(int id)
        {
            return await _productToppingRepository.DeleteProductToppingAsync(id);
        }

        public async Task<Response<ProductTopping>> GetProductToppingByProductId(int productId)
        {
            return await _productToppingRepository.GetProductToppingByProductIdAsync(productId);
        }

        public async Task<Response<ProductTopping>> GetProductToppingByToppingId(int toppingId)
        {
            return await _productToppingRepository.GetProductToppingByToppingIdAsync(toppingId);
        }
    }
}