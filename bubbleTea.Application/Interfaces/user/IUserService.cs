using BubbleTea.Domain.Entities;

namespace BubbleTea.Application.Interfaces
{
    public interface IUserService
    {
        Task<Response<IEnumerable<User>>> GetAllUser(int page, int pageSize);
        Task<Response<User>> GetUserById(int id);
        Task<Response<User>> CreateUser(User user);
        Task<Response<User>> UpdateUser(User user);
        Task<Response<User>> DeleteUser(int id);
        Task<Response<User>> LoginUser(string email, string password);
        Task<Response<User>> RegisterUser(User user);
        Task<Response<User>> UpdateUserPassword(int id, string oldPassword, string newPassword);
    }
}