using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;
using WebApi.Repository.Interfaces;

namespace WebApi.Repository
{
    public class LikesRepository : BaseRepository, ILikesRepository
    {
        public LikesRepository(DataContext context) : base(context)
        {
        }

        public async Task<OperationResultNew<PostLike>> GetLikeByPostIdAsync(int postId, string userId)
        {
            try
            {
                var result = await _context.PostLikes.FirstOrDefaultAsync(pl => pl.PostId == postId && pl.UserId == userId);

                if (result == null)
                {
                    return OperationResultNew<PostLike>.IsFailure("Post is not liked by user.");
                }

                return OperationResultNew<PostLike>.IsSuccess(result);
            }
            catch (Exception ex)
            {
                return OperationResultNew<PostLike>.IsFailure(ex.Message);
            };
        }

        public async Task<OperationResultNew<PostLike>> LikePostAsync(int postId, string userId)
        {
            if (postId <= 0 || string.IsNullOrEmpty(userId))
            {
                return OperationResultNew<PostLike>.IsFailure("Bad request.");
            }

            try
            {
                await _context.PostLikes.AddAsync(new PostLike { PostId = postId, UserId = userId });
                await _context.SaveChangesAsync();

                return OperationResultNew<PostLike>.IsSuccess(new PostLike { PostId = postId, UserId = userId });
            }
            catch (Exception ex)
            {
                return OperationResultNew<PostLike>.IsFailure(ex.Message);
            }
        }

        public async Task<OperationResultNew<CommentLike>> GetLikeByCommentIdAsync(int commentId, string userId)
        {
            try
            {
                var result = await _context.CommentLikes.FirstOrDefaultAsync(cl => cl.CommentId == commentId && cl.UserId == userId);

                if (result == null)
                {
                    return OperationResultNew<CommentLike>.IsFailure("Comment is not liked by user.");
                }

                return OperationResultNew<CommentLike>.IsSuccess(result);
            }
            catch (Exception ex)
            {
                return OperationResultNew<CommentLike>.IsFailure(ex.Message);
            }
        }

        public async Task<OperationResultNew<CommentLike>> LikeCommentAsync(int commentId, string userId)
        {
            if (commentId <= 0 || string.IsNullOrEmpty(userId))
            {
                return OperationResultNew<CommentLike>.IsFailure("Bad request.");
            }

            try
            {
                await _context.CommentLikes.AddAsync(new CommentLike { CommentId = commentId, UserId = userId });
                await _context.SaveChangesAsync();

                return OperationResultNew<CommentLike>.IsSuccess(new CommentLike { CommentId = commentId, UserId = userId });
            }
            catch (Exception ex)
            {
                return OperationResultNew<CommentLike>.IsFailure(ex.Message);
            }
        }

        public async Task<OperationResultNew<PostLike>> DeletePostLikeAsync(int postId, string userId)
        {
            var result = await _context.PostLikes.FirstOrDefaultAsync(pl => pl.PostId == postId && pl.UserId == userId);

            if (result == null)
            {
                return OperationResultNew<PostLike>.IsFailure("Post is not liked by user.");
            }

            try
            {
                _context.PostLikes.Remove(result);
                await _context.SaveChangesAsync();
                return OperationResultNew<PostLike>.IsSuccess(result);
            }
            catch (Exception ex)
            {
                return OperationResultNew<PostLike>.IsFailure(ex.Message);
            }
        }

        public async Task<OperationResultNew<CommentLike>> DeleteCommentLikeAsync(int commentId, string userId)
        {
            var result = await _context.CommentLikes.FirstOrDefaultAsync(cl => cl.CommentId == commentId && cl.UserId == userId);

            if (result == null)
            {
                return OperationResultNew<CommentLike>.IsFailure("Comment is not liked by user.");
            }

            try
            {
                _context.CommentLikes.Remove(result);
                await _context.SaveChangesAsync();
                return OperationResultNew<CommentLike>.IsSuccess(result);
            }
            catch (Exception ex)
            {
                return OperationResultNew<CommentLike>.IsFailure(ex.Message);
            }
        }
    }
}
