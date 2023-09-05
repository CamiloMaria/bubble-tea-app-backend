using BubbleTea.Domain.Entities;

namespace BubbleTea.Application.Interfaces
{
    public interface ILocationCityService
    {
        Task<Response<IEnumerable<City>>> GetAllLocationCity();
        Task<Response<City>> GetLocationCityById(int id);
        Task<Response<City>> CreateLocationCity(City city);
        Task<Response<City>> UpdateLocationCity(City city);
        Task<Response<City>> DeleteLocationCity(int id);
    }
}