using Common.Dto.CommentHistory;
using Common.Models;
using WebApi.Models;

namespace WebApi.Services.Interfaces
{
    public interface ICommentHistoryService
    {
        public Task<Result<CommentHistoriesDto>> GetCommentHistoryAsync(int commentId, PageInfo pageInfo);
    }
}
