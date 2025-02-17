using Common.Dto.User;
using Common.Models;
using MVC.Models;

namespace MVC.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<Result<UserDto>> GetUserAsync(string userName);
        public Task<Result<BearerToken>> LoginAsync(object data, string? bearerToken = default);
    }
}
