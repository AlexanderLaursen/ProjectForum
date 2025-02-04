using WebApi.Dto.Comment;
using WebApi.Models;

public interface ICommentRepository
{
    Task<OperationResult> GetCommentByIdAsync(int commentId);
    Task<OperationResult> GetCommentsByPostIdAsync(int postId, PageInfo pageInfo);
    Task<OperationResult> GetCommentsByUsernameAsync(string username, PageInfo pageInfo);
    Task<OperationResult> GetCommentHistoryById(int commentId, PageInfo pageInfo);
    Task<OperationResult> CreateCommentAsync(string userId, CreateCommentDto createCommentDto);
    Task<OperationResult> UpdateCommentAsync(string userId, UpdateCommentDto updateCommentDto);
    Task<OperationResult> DeleteCommentAsync(int commentId, string userId);
}