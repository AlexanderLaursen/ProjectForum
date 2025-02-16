using Common.Dto.Comment;
using Common.Models;

namespace WebApi.Services.Interfaces
{
    public interface ICommentService
    {
        public Task<Result<CommentDto>> GetCommentAsync(int commentId, string? userId);
    }
}