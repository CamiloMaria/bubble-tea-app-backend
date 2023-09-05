using BubbleTea.Domain.Entities;

namespace BubbleTea.Domain.Interfaces
{
    public interface ISizeRepository
    {
        Task<Response<IEnumerable<Size>>> GetAllSizeAsync(int page, int pageSize);
        Task<Response<Size>> GetSizeByIdAsync(int id);
        Task<Response<Size>> CreateSizeAsync(Size productSize);
        Task<Response<Size>> UpdateSizeAsync(Size productSize);
        Task<Response<Size>> DeleteSizeAsync(int id);        
    }
}