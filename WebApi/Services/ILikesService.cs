using WebApi.Models;

namespace WebApi.Services
{
    public interface ILikesService
    {
        public Task<OperationResultNew<PostLike>> LikePostAsync(int postId, string userId);
        public Task<OperationResultNew<CommentLike>> LikeCommentAsync(int commentId, string userId);
        public Task<OperationResultNew<PostLike>> DeletePostLikeAsync(int postId, string userId);
        public Task<OperationResultNew<CommentLike>> DeleteCommentLikeAsync(int commentId, string userId);
    }
}
