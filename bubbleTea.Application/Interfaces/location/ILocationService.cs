using BubbleTea.Domain.Entities;

namespace BubbleTea.Application.Interfaces
{
    public interface ILocationService
    {
        Task<Response<IEnumerable<Location>>> GetAllLocation(int page, int pageSize);
        Task<Response<Location>> GetLocationById(int id);
        Task<Response<Location>> CreateLocation(Location location);
        Task<Response<Location>> UpdateLocation(Location location);
        Task<Response<Location>> DeleteLocation(int id);
        Task<Response<Location>> GetLocationByUserId(int userId);
    }
}