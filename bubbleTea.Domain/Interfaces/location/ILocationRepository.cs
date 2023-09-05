using BubbleTea.Domain.Entities;

namespace BubbleTea.Domain.Interfaces
{
    public interface ILocationRepository
    {
        Task<Response<IEnumerable<Location>>> GetAllLocationAsync();
        Task<Response<Location>> GetLocationByIdAsync(int id);
        Task<Response<Location>> CreateLocationAsync(Location location);
        Task<Response<Location>> UpdateLocationAsync(Location location);
        Task<Response<Location>> DeleteLocationAsync(int id);
    }
}