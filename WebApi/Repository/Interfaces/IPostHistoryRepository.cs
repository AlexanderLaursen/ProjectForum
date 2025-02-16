using WebApi.Models;
using Common.Models;

namespace WebApi.Repository.Interfaces
{
    public interface IPostHistoryRepository
    {
        public Task<OperationResult> GetPostHistoryByIdAsync(int postId, PageInfo pageInfo);
    }
}
