using Common.Dto.Comment;
using WebApi.Models;
using Common.Models;

public interface ICommentRepository
{
    public Task<Result<CommentDto>> GetCommentAsync(int commentId, string? userId = null);
    public Task<Result<Comment>> GetFullCommentAsync(int commentId);
    public Task<Result<PagedCommentsDto>> GetCommentsByPostIdAsync(int postId, PageInfo pageInfo, string? userId);
    public Task<Result<PagedCommentsDto>> GetCommentsByUserIdAsync(string userId, PageInfo pageInfo);
    public Task<Result<CommentDto>> CreateCommentAsync(string userId, CreateCommentDto createCommentDto);
    public Task<Result<CommentDto>> UpdateCommentAsync(Comment comment, UpdateCommentDto updateCommentDto);
    public Task<Result <CommentDto>> DeleteCommentAsync(Comment comment);
}