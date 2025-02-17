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
        private readonly ICommonRepository _commonRepository;
        private readonly ILogger<CommentRepository> _logger;

        public CommentRepository(DataContext context, ICommonRepository commonRepository, ILogger<CommentRepository> logger)
        {
            _context = context;
            _commonRepository = commonRepository;
            _logger = logger;
            //_logger.LogInformation($"CommentRepository constructed with DataContext hash code: {_context.GetHashCode()}"); // Log hash code
        }

        public async Task<OperationResult> GetCommentByIdAsync(int commentId)
        {
            if (commentId <= 0)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = "Invalid comment id."
                };
            }

            try
            {
                var comment = await _context.Comments
                    .Include(p => p.User)
                    .FirstOrDefaultAsync(p => p.Id == commentId);

                if (comment == null)
                {
                    return new OperationResult
                    {
                        Success = false,
                        ErrorMessage = "Comment not found."
                    };
                }

                CommentDto commentDto = comment.Adapt<CommentDto>();
                List<CommentDto> commentList = [commentDto];

                return new OperationResult
                {
                    Success = true,
                    Data = new Dictionary<string, object>
                    {
                        { "content", commentList }
                    }
                };
            }
            catch (Exception ex)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = $"{ex.Message} - {ex.InnerException?.ToString()}"
                };
            }
        }

        public async Task<OperationResult> GetCommentsByPostIdAsync(int postId, PageInfo pageInfo)
        {
            if (postId <= 0)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = "Invalid comment id."
                };
            }

            try
            {
                IQueryable<Comment> query = _context.Comments
                    .Include(c => c.User)
                    .Where(c => c.PostId == postId)
                    .Skip(pageInfo.Skip)
                    .Take(pageInfo.PageSize);

                int totalItems = query.Count();

                List<Comment> comments = await query.ToListAsync();
                List<CommentDto> commentsDto = comments.Adapt<List<CommentDto>>();

                return new OperationResult
                {
                    Success = true,
                    Data = new Dictionary<string, object>
                    {
                        { "content", commentsDto },
                        { "pageInfo", new PageInfo
                            {
                                CurrentPage = pageInfo.CurrentPage,
                                PageSize = pageInfo.PageSize,
                                TotalItems = totalItems
                            }
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = $"{ex.Message} - {ex.InnerException?.ToString()}"
                };
            }
        }

        public async Task<OperationResult> GetCommentsByUsernameAsync(string username, PageInfo pageInfo)
        {
            if (string.IsNullOrEmpty(username))
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = "Invalid user id."
                };
            }

            string? userId = await _commonRepository.GetUserIdByUsernameAsync(username);

            if (string.IsNullOrEmpty(userId))
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = "User not found."
                };
            }

            try
            {
                var comments = await _context.Comments
                    .Include(c => c.User)
                    .Where(c => c.UserId == userId)
                    .Skip(pageInfo.Skip)
                    .Take(pageInfo.PageSize)
                    .ToListAsync();

                if (comments == null)
                {
                    return new OperationResult();
                }

                int totalItems = _context.Comments
                    .Where(c => c.UserId == userId)
                    .Count();

                List<CommentWithPostIdDto> commentsDto = comments.Adapt<List<CommentWithPostIdDto>>();

                return new OperationResult
                {
                    Success = true,
                    Data = new Dictionary<string, object>
                    {
                        { "content", commentsDto },
                        { "pageInfo", new PageInfo
                            {
                                CurrentPage = pageInfo.CurrentPage,
                                PageSize = pageInfo.PageSize,
                                TotalItems = totalItems
                            }
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = $"{ex.Message} - {ex.InnerException?.ToString()}"
                };
            }
        }

        public async Task<OperationResult> GetCommentHistoryById(int commentId, PageInfo pageInfo)
        {
            if (commentId <= 0)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = "Invalid comment id."
                };
            }

            try
            {
                IQueryable<CommentHistory> query = _context.CommentHistory
                    .Where(ch => ch.CommentId == commentId)
                    .Skip(pageInfo.Skip)
                    .Take(pageInfo.PageSize);

                int totalItems = query.Count();

                List<CommentHistory> commentHistory = await query.ToListAsync();
                List<CommentHistoryDto> commentHistoryDto = commentHistory.Adapt<List<CommentHistoryDto>>();

                return new OperationResult
                {
                    Success = true,
                    Data = new Dictionary<string, object>
                    {
                        { "content", commentHistoryDto },
                        { "pageInfo", new PageInfo
                            {
                                CurrentPage = pageInfo.CurrentPage,
                                PageSize = pageInfo.PageSize,
                                TotalItems = totalItems
                            }
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = $"{ex.Message} - {ex.InnerException?.ToString()}"
                };
            }
        }

        public async Task<OperationResult> CreateCommentAsync(string userId, CreateCommentDto createCommentDto)
        {
            try
            {
                Comment comment = createCommentDto.Adapt<Comment>();
                comment.UserId = userId;
                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();

                CommentDto commentDto = comment.Adapt<CommentDto>();
                List<CommentDto> commentList = [commentDto];

                return new OperationResult
                {
                    Success = true,
                    Data = new Dictionary<string, object>
                    {
                        { "content", commentList }
                    }
                };
            }
            catch (Exception ex)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = $"{ex.Message} - {ex.InnerException?.ToString()}"
                };
            }
        }

        public async Task<OperationResult> UpdateCommentAsync(string userId, UpdateCommentDto updateCommentDto)
        {
            if (updateCommentDto.CommentId <= 0)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = "Invalid comment id."
                };
            }

            if (userId == null)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = "Invalid user id."
                };
            }

            Comment? comment = await _context.Comments
                .Include(c => c.User)
                .FirstOrDefaultAsync(p => p.Id == updateCommentDto.CommentId);

            if (comment == null)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = "Comment not found."
                };
            }

            if (comment.UserId != userId)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = "Unauthorized user."
                };
            }

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

                    int commentHistoryId = commentHistory.Id;

                    comment.CommentHistory.Add(commentHistory);
                    comment.Edited = true;
                    comment.EditedAt = DateTime.UtcNow;
                    comment.Content = updateCommentDto.Content;
                    _context.Comments.Update(comment);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    CommentDto commentDto = comment.Adapt<CommentDto>();
                    List<CommentDto> commentDtoList = [commentDto];
                    return new OperationResult
                    {
                        Success = true,
                        Data = new Dictionary<string, object>
                        {
                            { "content", commentDtoList }
                        }
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    return new OperationResult
                    {
                        Success = false,
                        ErrorMessage = $"{ex.Message} - {ex.InnerException?.ToString()}"
                    };
                }
        }

        public async Task<OperationResult> DeleteCommentAsync(int commentId, string userId)
        {
            if (commentId <= 0)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = "Invalid comment id."
                };
            }

            if (userId == null)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = "Invalid user id."
                };
            }

            Comment? comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment == null)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = "Comment not found."
                };
            }

            if (comment.UserId != userId)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = "Unauthorized user."
                };
            }

            try
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();

                return new OperationResult
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = $"{ex.Message} - {ex.InnerException?.ToString()}"
                };
            }
        }

        // v2
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
    }
}
