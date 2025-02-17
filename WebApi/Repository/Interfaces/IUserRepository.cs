using Common.Dto.User;
using Common.Models;
using WebApi.Models;

namespace WebApi.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<Result<AppUser>> GetAppUserAsync(string userId);
        public Task<Result<UserDto>> GetUserAsync(string userName);
        public Task<Result<UserDto>> UpdateUserAsync(AppUser user);
    }
}
