using Mapster;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Dto.Comment;
using WebApi.Dto.CommentHistory;
using WebApi.Models;

namespace WebApi.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DataContext _context;
        private readonly ICommonRepository _commonRepository;

        public CommentRepository(DataContext context, ICommonRepository commonRepository)
        {
            _context = context;
            _commonRepository = commonRepository;
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
                comment.CreatedAt = DateTime.Now;
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
                        Content = updateCommentDto.Content,
                        CommentId = updateCommentDto.CommentId,
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
                    comment.EditedAt = DateTime.Now;
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

    }
}
