using WebApi.Models;

namespace WebApi.Repository
{
    public interface IPostHistoryRepository
    {
        public Task<OperationResult> GetPostHistoryByIdAsync(int postId, PageInfo pageInfo);
    }
}
