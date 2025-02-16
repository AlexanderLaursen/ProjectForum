using WebApi.Models;

namespace WebApi.Repository.Interfaces
{
    public interface IUserRepository
    {
        public Task<OperationResult> GetUserByUsernameAsync(string username);
        public Task<OperationResult> UpdateUserAsync(AppUser user);
    }
}
