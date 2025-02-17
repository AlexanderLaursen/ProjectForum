using Common.Models;
using WebApi.Models;

namespace WebApi.Services.Interfaces
{
    public interface ILikesService
    {
        public Task<Result<PostLike>> LikePostAsync(int postId, string userId);
        public Task<Result<CommentLike>> LikeCommentAsync(int commentId, string userId);
        public Task<Result<PostLike>> RemovePostLikeAsync(int postId, string userId);
        public Task<Result<CommentLike>> RemoveCommentLikeAsync(int commentId, string userId);
    }
}
