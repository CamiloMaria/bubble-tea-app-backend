using BubbleTea.Domain.Entities;

namespace BubbleTea.Application.Interfaces
{
    public interface ISizeService
    {
        Task<Response<IEnumerable<Size>>> GetAllSize(int page, int pageSize);
        Task<Response<Size>> GetSizeById(int id);
        Task<Response<Size>> CreateSize(Size Size);
        Task<Response<Size>> UpdateSize(Size Size);
        Task<Response<Size>> DeleteSize(int id);
    }
}
