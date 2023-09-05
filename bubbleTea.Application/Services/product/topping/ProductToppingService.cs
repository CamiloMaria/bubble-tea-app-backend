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

        public async Task<Response<IEnumerable<Topping>>> GetAllProductTopping(int page, int pageSize)
        {
            return await _productToppingRepository.GetAllProductToppingAsync(page, pageSize);
        }

        public async Task<Response<Topping>> GetProductToppingById(int id)
        {
            return await _productToppingRepository.GetProductToppingByIdAsync(id);
        }

        public async Task<Response<Topping>> CreateProductTopping(Topping productTopping)
        {
            return await _productToppingRepository.CreateProductToppingAsync(productTopping);
        }

        public async Task<Response<Topping>> UpdateProductTopping(Topping productTopping)
        {
            return await _productToppingRepository.UpdateProductToppingAsync(productTopping);
        }

        public async Task<Response<Topping>> DeleteProductTopping(int id)
        {
            return await _productToppingRepository.DeleteProductToppingAsync(id);
        }

        public async Task<Response<IEnumerable<Topping>>> GetProductToppingByProductId(int productId)
        {
            return await _productToppingRepository.GetProductToppingByProductIdAsync(productId);
        }
    }
}