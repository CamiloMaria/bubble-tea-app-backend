using BubbleTea.Domain.Entities;

namespace BubbleTea.Domain.Interfaces
{
    public interface ILocationCityRepository
    {
        Task<Response<IEnumerable<City>>> GetAllLocationCityAsync();
        Task<Response<City>> GetLocationCityByIdAsync(int id);
        Task<Response<City>> CreateLocationCityAsync(City city);
        Task<Response<City>> UpdateLocationCityAsync(City city);
        Task<Response<City>> DeleteLocationCityAsync(int id);
    }
}