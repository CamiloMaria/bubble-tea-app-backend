using BubbleTea.Domain.Entities;

namespace BubbleTea.Domain.Interfaces
{
    public interface IImageRepository
    {
        Task<Response<IEnumerable<Image>>> GetAllImageAsync(int page, int pageSize);
        Task<Response<Image>> GetImageByIdAsync(int id);
        Task<Response<Image>> CreateImageAsync(Image image);
        Task<Response<Image>> UpdateImageAsync(Image image);
        Task<Response<Image>> DeleteImageAsync(int id);
        Task<Response<IEnumerable<Image>>> GetImageByProductIdAsync(int productId);
        Task<Response<Image>> GetImageByUserIdAsync(int userId);
    }
}