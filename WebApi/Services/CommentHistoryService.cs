using Common.Dto.CommentHistory;
using Common.Models;
using WebApi.Models;
using WebApi.Repository.Interfaces;
using WebApi.Services.Interfaces;

namespace WebApi.Services
{
    public class CommentHistoryService : ICommentHistoryService
    {
        private readonly ICommentHistoryRepository _commentHistoryRepository;
        public CommentHistoryService(ICommentHistoryRepository commentHistoryRepository)
        {
            _commentHistoryRepository = commentHistoryRepository;
        }

        public async Task<Result<CommentHistoriesDto>> GetCommentHistoryAsync(int commentId, PageInfo pageInfo)
        {
            return await _commentHistoryRepository.GetCommentHistoryAsync(commentId, pageInfo);
        }
    }
}
