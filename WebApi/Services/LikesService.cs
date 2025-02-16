using WebApi.Models;
using WebApi.Repository.Interfaces;
using Common.Models;
using WebApi.Services.Interfaces;

namespace WebApi.Services
{
    public class LikesService : ILikesService
    {
        private readonly ILikesRepository _likesRepository;
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        public LikesService(ILikesRepository likesRepository, IPostRepository postRepository, ICommentRepository commentRepository)
        {
            _likesRepository = likesRepository;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
        }

        public async Task<OperationResultNew<PostLike>> LikePostAsync(int postId, string userId)
        {
            if (postId <= 0 || string.IsNullOrEmpty(userId))
            {
                return OperationResultNew<PostLike>.IsFailure("Bad request.");
            }

            OperationResult postResult = await _postRepository.GetPostByIdAsync(postId, new PageInfo());

            if (!postResult.Success)
            {
                return OperationResultNew<PostLike>.IsFailure("Post not found.");
            }

            var like = await _likesRepository.GetLikeByPostIdAsync(postId, userId);

            if (like.Success)
            {
                return OperationResultNew<PostLike>.IsFailure("Post is already liked by user.");
            }

            return await _likesRepository.LikePostAsync(postId, userId);
        }

        public async Task<OperationResultNew<CommentLike>> LikeCommentAsync(int commentId, string userId)
        {
            if (commentId <= 0 || string.IsNullOrEmpty(userId))
            {
                return OperationResultNew<CommentLike>.IsFailure("Bad request.");
            }

            OperationResult commentResult = await _commentRepository.GetCommentByIdAsync(commentId);

            if (!commentResult.Success)
            {
                return OperationResultNew<CommentLike>.IsFailure("Comment not found.");
            }

            var like = await _likesRepository.GetLikeByCommentIdAsync(commentId, userId);

            if (like.Success)
            {
                return OperationResultNew<CommentLike>.IsFailure("Comment is already liked by user.");
            }

            return await _likesRepository.LikeCommentAsync(commentId, userId);
        }

        public async Task<OperationResultNew<PostLike>> DeletePostLikeAsync(int postId, string userId)
        {
            if (postId <= 0 || string.IsNullOrEmpty(userId))
            {
                return OperationResultNew<PostLike>.IsFailure("Bad request.");
            }

            return await _likesRepository.DeletePostLikeAsync(postId, userId);
        }

        public async Task<OperationResultNew<CommentLike>> DeleteCommentLikeAsync(int commentId, string userId)
        {
            if (commentId <= 0 || string.IsNullOrEmpty(userId))
            {
                return OperationResultNew<CommentLike>.IsFailure("Bad request.");
            }

            return await _likesRepository.DeleteCommentLikeAsync(commentId, userId);
        }
    }
}
