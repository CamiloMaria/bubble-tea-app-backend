using BubbleTea.Domain.Entities;

namespace BubbleTea.Application.Interfaces
{
    public interface IUserService
    {
        Task<Response<IEnumerable<User>>> GetAllUser();
        Task<Response<User>> GetUserById(int id);
        Task<Response<User>> AddUser(User user);
        Task<Response<User>> UpdateUSer(User user);
        Task<Response<User>> DeleteUser(int id);
        Task<Response<User>> LoginUser(string email, string password);
        Task<Response<User>> RegisterUser(User user);
        Task<Response<User>> UpdateUserPassword(int id, string oldPassword, string newPassword);
    }
}