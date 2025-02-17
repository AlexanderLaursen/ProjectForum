using Common.Dto.User;
using Common.Models;
using WebApi.Models;

namespace WebApi.Services.Interfaces
{
    public interface IUserService
    {
        public Task<Result<UserDto>> GetUserAsync(string userName);
        public Task<Result<AppUser>> GetAppUserAsync(string userId);
        public Task<Result<UserDto>> UpdateUserAsync(AppUser user);
    }
}
