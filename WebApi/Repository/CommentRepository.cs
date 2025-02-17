using Mapster;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using Common.Dto.Comment;
using Common.Dto.CommentHistory;
using WebApi.Models;
using WebApi.Repository.Interfaces;
using Common.Models;
using Common.Dto.User;

namespace WebApi.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<CommentRepository> _logger;

        public CommentRepository(DataContext context, ILogger<CommentRepository> logger)
        {
            _context = context;
            _logger = logger;
            //_logger.LogInformation($"CommentRepository constructed with DataContext hash code: {_context.GetHashCode()}");
        }

        public async Task<Result<CommentDto>> GetCommentAsync(int commentId, string? userId = null)
        {
            try
            {
                CommentDto? commentDto = await _context.Comments
                    .Where(c => c.Id == commentId)
                    .Select(c => new CommentDto
                    {
                        Id = c.Id,
                        Content = c.Content,
                        CreatedAt = c.CreatedAt,
                        Likes = c.CommentLikes.Count,
                        PostId = c.PostId,
                        Edited = c.Edited,
                        LikedByUser = c.CommentLikes.Any(cl => cl.UserId == userId),
                        UserId = c.UserId,
                        User = c.User.Adapt<ShortUserDto>()
                    }).FirstOrDefaultAsync();

                if (commentDto == null)
                {
                    return Result<CommentDto>.NotFound();
                }

                return Result<CommentDto>.Success(commentDto);
            }
            catch (Exception)
            {
                _logger.LogError("Error while fetching comment from database.");
                return Result<CommentDto>.Failure("Error while fetching comment from database.");
            }
        }

        public async Task<Result<Comment>> GetFullCommentAsync(int commentId)
        {
            try
            {
                Comment? comment = await _context.Comments
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.Id == commentId);

                if (comment == null)
                {
                    return Result<Comment>.NotFound();
                }

                return Result<Comment>.Success(comment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching comment from database.");
                return Result<Comment>.Failure("Error while fetching comment from database.");
            }
        }

        public async Task<Result<PagedCommentsDto>> GetCommentsByPostIdAsync(int postId, PageInfo pageInfo, string? userId)
        {
            try
            {
                var query = _context.Comments.Where(c => c.PostId == postId);

                int totalItems = await query.CountAsync();

                if (totalItems == 0)
                {
                    return Result<PagedCommentsDto>.NotFound();
                }

                List<CommentDto> commentsDto = await query
                    .Select(c => new CommentDto
                    {
                        Id = c.Id,
                        Content = c.Content,
                        CreatedAt = c.CreatedAt,
                        EditedAt = c.EditedAt,
                        Likes = c.CommentLikes.Count,
                        PostId = c.PostId,
                        Edited = c.Edited,
                        LikedByUser = c.CommentLikes.Any(cl => cl.UserId == userId),
                        UserId = c.UserId,
                        User = c.User.Adapt<ShortUserDto>()
                    })
                    .Skip(pageInfo.Skip)
                    .Take(pageInfo.PageSize)
                    .ToListAsync();

                PagedCommentsDto commentListDto = new()
                {
                    Comments = commentsDto,
                    PageInfo = new PageInfo
                    {
                        CurrentPage = pageInfo.CurrentPage,
                        PageSize = pageInfo.PageSize,
                        TotalItems = totalItems
                    }
                };

                return Result<PagedCommentsDto>.Success(commentListDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching comments from database.");
                return Result<PagedCommentsDto>.Failure("Error while fetching comments from database.");
            }
        }

        public async Task<Result<PagedCommentsDto>> GetCommentsByUserIdAsync(string userId, PageInfo pageInfo)
        {
            try
            {
                var query = _context.Comments
                    .Where(c => c.UserId == userId);

                int totalItems = await query.CountAsync();

                if (totalItems == 0)
                {
                    return Result<PagedCommentsDto>.NotFound();
                };

                List<CommentDto> commentsDto = await query
                    .Select(c => new CommentDto
                    {
                        Id = c.Id,
                        Content = c.Content,
                        CreatedAt = c.CreatedAt,
                        EditedAt = c.EditedAt,
                        Likes = c.CommentLikes.Count,
                        PostId = c.PostId,
                        Edited = c.Edited,
                        LikedByUser = c.CommentLikes.Any(cl => cl.UserId == userId),
                        UserId = c.UserId,
                        User = c.User.Adapt<ShortUserDto>()
                    })
                    .Skip(pageInfo.Skip)
                    .Take(pageInfo.PageSize)
                    .ToListAsync();

                PagedCommentsDto commentListDto = new()
                {
                    Comments = commentsDto,
                    PageInfo = new PageInfo
                    {
                        CurrentPage = pageInfo.CurrentPage,
                        PageSize = pageInfo.PageSize,
                        TotalItems = totalItems
                    }
                };

                return Result<PagedCommentsDto>.Success(commentListDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching comments from database.");
                return Result<PagedCommentsDto>.Failure("Error while fetching comments from database.");
            }
        }

        public async Task<Result<CommentDto>> CreateCommentAsync(string userId, CreateCommentDto createCommentDto)
        {
            try
            {
                Comment comment = createCommentDto.Adapt<Comment>();
                comment.UserId = userId;
                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();

                CommentDto commentDto = comment.Adapt<CommentDto>();

                return Result<CommentDto>.Success(commentDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating comment.");
                return Result<CommentDto>.Failure("Error while creating comment.");
            }
        }

        public async Task<Result<CommentDto>> UpdateCommentAsync(Comment comment, UpdateCommentDto updateCommentDto)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
                try
                {
                    CommentHistory commentHistory = new()
                    {
                        Content = comment.Content,
                        CommentId = comment.Id,
                        UserId = comment.UserId,
                        CreatedAt = comment.EditedAt == DateTime.MinValue ? comment.CreatedAt : comment.EditedAt,
                        User = comment.User,
                        Comment = comment
                    };

                    _context.CommentHistory.Add(commentHistory);
                    await _context.SaveChangesAsync();

                    comment.Edited = true;
                    comment.EditedAt = DateTime.UtcNow;
                    comment.Content = updateCommentDto.Content;
                    _context.Comments.Update(comment);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return Result<CommentDto>.Success(comment.Adapt<CommentDto>());
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    _logger.LogError(ex, "Error while updating comment.");
                    return Result<CommentDto>.Failure("Error while updating comment.");
                }
        }

        public async Task<Result<CommentDto>> DeleteCommentAsync(Comment comment)
        {
            try
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();

                return Result<CommentDto>.Success(comment.Adapt<CommentDto>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting comment.");
                return Result<CommentDto>.Failure("Error while deleting comment.");
            }
        }
    }
}
