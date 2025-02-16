using Common.Dto.Comment;
using WebApi.Models;
using Common.Models;

public interface ICommentRepository
{
    public Task<OperationResult> GetCommentByIdAsync(int commentId);
    public Task<OperationResult> GetCommentsByPostIdAsync(int postId, PageInfo pageInfo);
    public Task<OperationResult> GetCommentsByUsernameAsync(string username, PageInfo pageInfo);
    public Task<OperationResult> GetCommentHistoryById(int commentId, PageInfo pageInfo);
    public Task<OperationResult> CreateCommentAsync(string userId, CreateCommentDto createCommentDto);
    public Task<OperationResult> UpdateCommentAsync(string userId, UpdateCommentDto updateCommentDto);
    public Task<OperationResult> DeleteCommentAsync(int commentId, string userId);

    public Task<Result<CommentDto>> GetCommentAsync(int commentId, string? userId = null);
}