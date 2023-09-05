using BubbleTea.Domain.Entities;
using BubbleTea.Domain.Interfaces;
using BubbleTea.Application.Interfaces;

namespace BubbleTea.Application.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;

        public LocationService(ILocationRepository locationRepository)
        {
            this._locationRepository = locationRepository;
        }

        public async Task<Response<IEnumerable<Location>>> GetAllLocation(int page, int pageSize)
        {
            return await _locationRepository.GetAllLocationAsync(page, pageSize);
        }

        public async Task<Response<Location>> GetLocationById(int id)
        {
            return await _locationRepository.GetLocationByIdAsync(id);
        }

        public async Task<Response<Location>> CreateLocation(Location location)
        {
            return await _locationRepository.CreateLocationAsync(location);
        }

        public async Task<Response<Location>> UpdateLocation(Location location)
        {
            return await _locationRepository.UpdateLocationAsync(location);
        }

        public async Task<Response<Location>> DeleteLocation(int id)
        {
            return await _locationRepository.DeleteLocationAsync(id);
        }

        public async Task<Response<Location>> GetLocationByUserId(int userId)
        {
            return await _locationRepository.GetLocationByUserIdAsync(userId);
        }
    }
}