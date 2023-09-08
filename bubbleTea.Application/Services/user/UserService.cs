using BubbleTea.Application.Interfaces;
using BubbleTea.Domain.Entities;
using BubbleTea.Domain.Interfaces;

namespace BubbleTea.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Response<IEnumerable<User>>> GetAllUser(int page, int pageSize)
        {
            return await _userRepository.GetAllUserAsync(page, pageSize);
        }

        public async Task<Response<User>> GetUserById(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<Response<User>> CreateUser(User user)
        {
            return await _userRepository.CreateUserAsync(user);
        }

        public async Task<Response<User>> UpdateUser(User user)
        {
            return await _userRepository.UpdateUserAsync(user);
        }

        public async Task<Response<User>> DeleteUser(int id)
        {
            return await _userRepository.DeleteUserAsync(id);
        }

        public async Task<Response<User>> LoginUser(string email, string password)
        {
            return await _userRepository.LoginUserAsync(email, password);
        }

        public async Task<Response<User>> RegisterUser(User user)
        {
            return await _userRepository.RegisterUserAsync(user);
        }

        public async Task<Response<User>> UpdateUserPassword(int id, string oldPassword, string newPassword)
        {
            return await _userRepository.UpdateUserPasswordAsync(id, oldPassword, newPassword);
        }
    }
}