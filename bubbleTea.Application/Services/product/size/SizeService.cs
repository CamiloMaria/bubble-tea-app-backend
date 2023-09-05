using BubbleTea.Domain.Entities;
using BubbleTea.Domain.Interfaces;
using BubbleTea.Application.Interfaces;

namespace BubbleTea.Application.Services
{
    public class SizeService : ISizeService
    {
        private readonly ISizeRepository _sizeRepository;

        public SizeService(ISizeRepository sizeRepository)
        {
            _sizeRepository = sizeRepository;
        }

        public async Task<Response<IEnumerable<Size>>> GetAllSize(int page, int pageSize)
        {
            return await _sizeRepository.GetAllSizeAsync(page, pageSize);
        }

        public async Task<Response<Size>> GetSizeById(int id)
        {
            return await _sizeRepository.GetSizeByIdAsync(id);
        }

        public async Task<Response<Size>> CreateSize(Size size)
        {
            return await _sizeRepository.CreateSizeAsync(size);
        }

        public async Task<Response<Size>> UpdateSize(Size size)
        {
            return await _sizeRepository.UpdateSizeAsync(size);
        }

        public async Task<Response<Size>> DeleteSize(int id)
        {
            return await _sizeRepository.DeleteSizeAsync(id);
        }
    }
}