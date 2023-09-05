using BubbleTea.Domain.Entities;

namespace BubbleTea.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<Response<IEnumerable<User>>> GetAllProductAsync();
        Task<Response<User>> GetUserByIdAsync(int id);
        Task<Response<User>> AddUserAsync(User user);
        Task<Response<User>> UpdateUSerAsync(User user);
        Task<Response<User>> DeleteUserAsync(int id);
        Task<Response<User>> LoginUserAsync(string email, string password);
        Task<Response<User>> RegisterUserAsync(User user);
        Task<Response<User>> UpdateUserPasswordAsync(int id, string oldPassword, string newPassword);
    }
}