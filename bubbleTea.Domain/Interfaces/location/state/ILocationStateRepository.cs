using BubbleTea.Domain.Entities;

namespace BubbleTea.Domain.Interfaces
{
    public interface ILocationStateRepository
    {
        Task<Response<IEnumerable<State>>> GetAllLocationStateAsync();
        Task<Response<State>> GetLocationStateByIdAsync(int id);
        Task<Response<State>> CreateLocationStateAsync(State state);
        Task<Response<State>> UpdateLocationStateAsync(State state);
        Task<Response<State>> DeleteLocationStateAsync(int id);
    }
}