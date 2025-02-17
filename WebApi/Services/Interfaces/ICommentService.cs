using Common.Dto.Comment;
using Common.Dto.Post;
using Common.Models;

namespace WebApi.Services.Interfaces
{
    public interface ICommentService
    {
        public Task<Result<CommentDto>> GetCommentAsync(int commentId, string? userId);
        public Task<Result<PagedCommentsDto>> GetCommentsByPostIdAsync(int postId, PageInfo pageInfo, string? userId);
        public Task<Result<PagedCommentsDto>> GetCommentsByUsernameAsync(string username, PageInfo pageInfo);
        public Task<Result<CommentDto>> CreateCommentAsync(string userId, CreateCommentDto createCommentDto);
        public Task<Result<CommentDto>> DeleteCommentAsync(int commentId, string userId);
        public Task<Result<CommentDto>> UpdateCommentAsync(int commentId, string userId, UpdateCommentDto updateCommentDto);
    }
}