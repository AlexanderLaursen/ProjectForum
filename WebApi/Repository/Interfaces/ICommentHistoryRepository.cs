using WebApi.Models;
using Common.Models;
using Common.Dto.CommentHistory;

namespace WebApi.Repository.Interfaces
{
    public interface ICommentHistoryRepository
    {
        public Task<Result<CommentHistoriesDto>> GetCommentHistoryAsync(int commentId, PageInfo pageInfo);
    }
}
