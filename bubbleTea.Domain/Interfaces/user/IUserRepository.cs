using BubbleTea.Domain.Entities;

namespace BubbleTea.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<Response<IEnumerable<User>>> GetAllUserAsync(int page, int pageSize);
        Task<Response<User>> GetUserByIdAsync(int id);
        Task<Response<User>> CreateUserAsync(User user);
        Task<Response<User>> UpdateUserAsync(User user);
        Task<Response<User>> DeleteUserAsync(int id);
        Task<Response<User>> LoginUserAsync(string email, string password);
        Task<Response<User>> RegisterUserAsync(User user);
        Task<Response<User>> UpdateUserPasswordAsync(int id, string oldPassword, string newPassword);
    }
}