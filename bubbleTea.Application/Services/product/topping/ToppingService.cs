using BubbleTea.Domain.Entities;
using BubbleTea.Domain.Interfaces;
using BubbleTea.Application.Interfaces;

namespace BubbleTea.Application.Services
{
    public class ToppingService : IToppingService
    {
        private readonly IToppingRepository _toppingRepository;

        public ToppingService(IToppingRepository toppingRepository)
        {
            _toppingRepository = toppingRepository;
        }

        public async Task<Response<IEnumerable<Topping>>> GetAllTopping(int page, int pageSize)
        {
            return await _toppingRepository.GetAllToppingAsync(page, pageSize);
        }

        public async Task<Response<Topping>> GetToppingById(int id)
        {
            return await _toppingRepository.GetToppingByIdAsync(id);
        }

        public async Task<Response<Topping>> CreateTopping(Topping topping)
        {
            return await _toppingRepository.CreateToppingAsync(topping);
        }

        public async Task<Response<Topping>> UpdateTopping(Topping topping)
        {
            return await _toppingRepository.UpdateToppingAsync(topping);
        }

        public async Task<Response<Topping>> DeleteTopping(int id)
        {
            return await _toppingRepository.DeleteToppingAsync(id);
        }
    }
}