using Common.Dto.CommentHistory;
using Common.Models;
using MVC.Models;

namespace MVC.Services.Interfaces
{
    public interface ICommentHistoryApiService
    {
        public Task<Result<CommentHistories>> GetCommentHistoryAsync(int commentId, PageInfo pageInfo);
    }
}