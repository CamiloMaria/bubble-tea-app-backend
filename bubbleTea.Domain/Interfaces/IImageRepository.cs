using BubbleTea.Domain.Entities;

namespace BubbleTea.Domain.Interfaces
{
    public interface IImageRepository
    {
        Task<Response<IEnumerable<Image>>> GetAllImageAsync();
        Task<Response<Image>> GetImageByIdAsync(int id);
        Task<Response<Image>> CreateImageAsync(Image image);
        Task<Response<Image>> UpdateImageAsync(Image image);
        Task<Response<Image>> DeleteImageAsync(int id);
    }
}