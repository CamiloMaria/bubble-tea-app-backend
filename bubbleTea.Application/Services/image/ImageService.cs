using BubbleTea.Domain.Entities;
using BubbleTea.Domain.Interfaces;
using BubbleTea.Application.Interfaces;

namespace BubbleTea.Application.Services
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;

        public ImageService(IImageRepository imageRepository)
        {
            this._imageRepository = imageRepository;
        }

        public async Task<Response<IEnumerable<Image>>> GetAllImage(int page, int pageSize)
        {
            return await _imageRepository.GetAllImageAsync(page, pageSize);
        }

        public async Task<Response<Image>> GetImageById(int id)
        {
            return await _imageRepository.GetImageByIdAsync(id);
        }

        public async Task<Response<Image>> CreateImage(Image image)
        {
            return await _imageRepository.CreateImageAsync(image);
        }

        public async Task<Response<Image>> UpdateImage(Image image)
        {
            return await _imageRepository.UpdateImageAsync(image);
        }

        public async Task<Response<Image>> DeleteImage(int id)
        {
            return await _imageRepository.DeleteImageAsync(id);
        }

        public async Task<Response<Image>> GetImageByProductId(int productId)
        {
            return await _imageRepository.GetImageByProductIdAsync(productId);
        }

        public async Task<Response<Image>> GetImageByUserId(int userId)
        {
            return await _imageRepository.GetImageByUserIdAsync(userId);
        }
    }
}