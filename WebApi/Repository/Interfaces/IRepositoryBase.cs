using Common.Models;

namespace WebApi.Repository.Interfaces
{
    public interface IBaseRepository
    {
        public Task IncrementViewCountAsync(int postId);
    }
}
