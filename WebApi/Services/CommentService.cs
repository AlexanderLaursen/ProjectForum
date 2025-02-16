using Common.Dto.Comment;
using Common.Models;
using WebApi.Services.Interfaces;

namespace WebApi.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }
        public async Task<Result<CommentDto>> GetCommentAsync(int commentId, string? userId)
        {
            return await _commentRepository.GetCommentAsync(commentId, userId);
        }
    }
}
