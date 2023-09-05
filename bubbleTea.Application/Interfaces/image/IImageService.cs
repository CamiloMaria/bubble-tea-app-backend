using BubbleTea.Domain.Entities;

namespace BubbleTea.Application.Interfaces
{
    public interface IImageService
    {
        Task<Response<IEnumerable<Image>>> GetAllImage();
        Task<Response<Image>> GetImageById(int id);
        Task<Response<Image>> CreateImage(Image image);
        Task<Response<Image>> UpdateImage(Image image);
        Task<Response<Image>> DeleteImage(int id);
    }
}