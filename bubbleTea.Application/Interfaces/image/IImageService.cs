using BubbleTea.Domain.Entities;

namespace BubbleTea.Application.Interfaces
{
    public interface IImageService
    {
        Task<Response<IEnumerable<Image>>> GetAllImage(int page, int pageSize);
        Task<Response<Image>> GetImageById(int id);
        Task<Response<Image>> CreateImage(Image image);
        Task<Response<Image>> UpdateImage(Image image);
        Task<Response<Image>> DeleteImage(int id);
        Task<Response<IEnumerable<Image>>> GetImageByProductId(int productId);
        Task<Response<Image>> GetImageByUserId(int userId);
    }
}