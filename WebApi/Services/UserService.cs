using Common.Dto.User;
using Common.Models;
using WebApi.Models;
using WebApi.Repository.Interfaces;
using WebApi.Services.Interfaces;

namespace WebApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<AppUser>> GetAppUserAsync(string userId)
        {
            return await _userRepository.GetAppUserAsync(userId);
        }

        public async Task<Result<UserDto>> GetUserAsync(string userName)
        {
            return await _userRepository.GetUserAsync(userName);
        }

        public async Task<Result<UserDto>> UpdateUserAsync(AppUser user)
        {
            return await _userRepository.UpdateUserAsync(user);
        }
    }
}
