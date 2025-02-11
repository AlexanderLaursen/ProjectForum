using WebApi.Models;

namespace WebApi.Repository
{
    public interface ICommentHistoryRepository
    {
        public Task<OperationResult> GetCommentHistoryByIdAsync(int commentId, PageInfo pageInfo);
    }
}
