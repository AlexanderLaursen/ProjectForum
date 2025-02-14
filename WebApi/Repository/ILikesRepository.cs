using WebApi.Models;

namespace WebApi.Repository
{
    public interface ILikesRepository
    {
        public Task<OperationResultNew<PostLike>> LikePostAsync(int postId, string userId);
        public Task<OperationResultNew<PostLike>> GetLikeByPostIdAsync(int postId, string userId);
        public Task<OperationResultNew<CommentLike>> LikeCommentAsync(int commentId, string userId);
        public Task<OperationResultNew<CommentLike>> GetLikeByCommentIdAsync(int commentId, string userId);
        public Task<OperationResultNew<PostLike>> DeletePostLikeAsync(int postId, string userId);
        public Task<OperationResultNew<CommentLike>> DeleteCommentLikeAsync(int commentId, string userId);
    }
}