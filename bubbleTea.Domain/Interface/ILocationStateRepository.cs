using BubbleTea.Domain.Entities;

namespace BubbleTea.Domain.Interfaces
{
    public interface ILocationStateRepository
    {
        Task<Response<IEnumerable<State>>> GetAllStateAsync();
        Task<Response<State>> GetStateByIdAsync(int id);
        Task<Response<State>> CreateStateAsync(State state);
        Task<Response<State>> UpdateStateAsync(State state);
        Task<Response<State>> DeleteStateAsync(int id);
    }
}