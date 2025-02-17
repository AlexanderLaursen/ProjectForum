using Common.Models;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;
using WebApi.Repository.Interfaces;

namespace WebApi.Repository
{
    public class LikesRepository : ILikesRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<LikesRepository> _logger;
        public LikesRepository(DataContext context, ILogger<LikesRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<PostLike>> GetLikePostAsync(int postId, string userId)
        {
            try
            {
                var result = await _context.PostLikes.FirstOrDefaultAsync(pl => pl.PostId == postId && pl.UserId == userId);
                
                if (result == null)
                {
                    return Result<PostLike>.NotFound("Post is not liked by user.");
                }

                return Result<PostLike>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while getting post like.");
                return Result<PostLike>.Failure("Error occured while getting post like.");
            }
        }

        public async Task<Result<PostLike>> LikePostAsync(int postId, string userId)
        {
            try
            {
                PostLike postLike = new PostLike { PostId = postId, UserId = userId };
                await _context.PostLikes.AddAsync(postLike);
                int changes = await _context.SaveChangesAsync();
                
                if (changes == 0)
                {
                    return Result<PostLike>.Failure("Unexpected error occured.");
                }

                return Result<PostLike>.Success(postLike);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while liking post.");
                return Result<PostLike>.Failure("Error occured while liking post.");
            }
        }

        public async Task<Result<CommentLike>> GetLikeCommentAsync(int commentId, string userId)
        {
            try
            {
                var result = await _context.CommentLikes.FirstOrDefaultAsync(cl => cl.CommentId == commentId && cl.UserId == userId);

                if (result == null)
                {
                    return Result<CommentLike>.NotFound("Comment is not liked by user.");
                }

                return Result<CommentLike>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while getting comment like.");
                return Result<CommentLike>.Failure("Errpr occured while getting comment like.");
            }
        }

        public async Task<Result<CommentLike>> LikeCommentAsync(int commentId, string userId)
        {
            try
            {
                CommentLike commentLike = new CommentLike { CommentId = commentId, UserId = userId };
                await _context.CommentLikes.AddAsync(commentLike);
                int changes = await _context.SaveChangesAsync();

                if (changes == 0)
                {
                    return Result<CommentLike>.Failure("Unexpected error occured.");
                }

                return Result<CommentLike>.Success(commentLike);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while liking comment.");
                return Result<CommentLike>.Failure("Error occured while liking comment.");
            }
        }

        public async Task<Result<PostLike>> DeletePostLikeAsync(int postId, string userId)
        {
            var result = await _context.PostLikes.FirstOrDefaultAsync(pl => pl.PostId == postId && pl.UserId == userId);

            if (result == null)
            {
                return Result<PostLike>.NotFound("Post is not liked by user.");
            }

            try
            {
                _context.PostLikes.Remove(result);
                await _context.SaveChangesAsync();
                return Result<PostLike>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while deleting post like.");
                return Result<PostLike>.Failure("Error occured while deleting post like.");
            }
        }

        public async Task<Result<CommentLike>> DeleteCommentLikeAsync(int commentId, string userId)
        {
            var result = await _context.CommentLikes.FirstOrDefaultAsync(cl => cl.CommentId == commentId && cl.UserId == userId);

            if (result == null)
            {
                return Result<CommentLike>.Failure("Comment is not liked by user.");
            }

            try
            {
                _context.CommentLikes.Remove(result);
                await _context.SaveChangesAsync();
                return Result<CommentLike>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while deleting comment like.");
                return Result<CommentLike>.Failure("Error occured while deleting comment like.");
            }
        }
    }
}
