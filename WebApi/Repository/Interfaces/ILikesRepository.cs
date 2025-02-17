using Common.Models;
using WebApi.Models;

namespace WebApi.Repository.Interfaces
{
    public interface ILikesRepository
    {
        public Task<Result<PostLike>> LikePostAsync(int postId, string userId);
        public Task<Result<CommentLike>> LikeCommentAsync(int commentId, string userId);
        public Task<Result<PostLike>> GetLikePostAsync(int postId, string userId);
        public Task<Result<CommentLike>> GetLikeCommentAsync(int commentId, string userId);
        public Task<Result<PostLike>> DeletePostLikeAsync(int postId, string userId);
        public Task<Result<CommentLike>> DeleteCommentLikeAsync(int commentId, string userId);
    }
}