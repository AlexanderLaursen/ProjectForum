using WebApi.Models;

namespace WebApi.Repository
{
    public interface IUserRepository
    {
        public Task<OperationResult> GetUserByUsernameAsync(string username);
    }
}
