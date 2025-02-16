using WebApi.Models;
using Common.Models;

namespace WebApi.Repository.Interfaces
{
    public interface ICommentHistoryRepository
    {
        public Task<OperationResult> GetCommentHistoryByIdAsync(int commentId, PageInfo pageInfo);
    }
}
