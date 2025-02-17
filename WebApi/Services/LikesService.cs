using WebApi.Models;
using WebApi.Repository.Interfaces;
using Common.Models;
using WebApi.Services.Interfaces;
using Common.Dto.Post;
using Common.Dto.Comment;
using System.Runtime.CompilerServices;

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

        public async Task<Result<PostLike>> LikePostAsync(int postId, string userId)
        {
            Result<PostDto> postResult = await _postRepository.GetPostAsync(postId);

            if (!postResult.IsSuccess)
            {
                return Result<PostLike>.NotFound("Post not found.");
            }

            Result<PostLike> like = await _likesRepository.GetLikePostAsync(postId, userId);

            if (like.IsSuccess)
            {
                return Result<PostLike>.Failure("Post is already liked by user.");
            }

            return await _likesRepository.LikePostAsync(postId, userId);
        }

        public async Task<Result<CommentLike>> LikeCommentAsync(int commentId, string userId)
        {
            Result<CommentDto> commentResult = await _commentRepository.GetCommentAsync(commentId);

            if (!commentResult.IsSuccess)
            {
                return Result<CommentLike>.Failure("Comment not found.");
            }

            Result<CommentLike> like = await _likesRepository.GetLikeCommentAsync(commentId, userId);

            if (like.IsSuccess)
            {
                return Result<CommentLike>.Failure("Comment is already liked by user.");
            }

            return await _likesRepository.LikeCommentAsync(commentId, userId);
        }

        public async Task<Result<PostLike>> RemovePostLikeAsync(int postId, string userId)
        {
            return await _likesRepository.DeletePostLikeAsync(postId, userId);
        }

        public async Task<Result<CommentLike>> RemoveCommentLikeAsync(int commentId, string userId)
        {
            return await _likesRepository.DeleteCommentLikeAsync(commentId, userId);
        }
    }
}
