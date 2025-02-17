using Common.Models;
using MVC.Models;

namespace MVC.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        public Task<Result<BearerToken>> LoginAsync(object data, string? bearerToken = default);
    }
}
