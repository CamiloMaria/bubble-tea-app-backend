using BubbleTea.Domain.Entities;

namespace BubbleTea.Application.Interfaces
{
    public interface ILocationStateService
    {
        Task<Response<IEnumerable<State>>> GetAllLocationState();
        Task<Response<State>> GetLocationStateById(int id);
        Task<Response<State>> CreateLocationState(State state);
        Task<Response<State>> UpdateLocationState(State state);
        Task<Response<State>> DeleteLocationState(int id);
    }
}