using Mapster;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using Common.Dto.CommentHistory;
using WebApi.Models;
using WebApi.Repository.Interfaces;
using Common.Models;

namespace WebApi.Repository
{
    public class CommentHistoryRepository : ICommentHistoryRepository
    {
        private readonly DataContext _context;

        public CommentHistoryRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<OperationResult> GetCommentHistoryByIdAsync(int commentId, PageInfo pageInfo)
        {
            try
            {
                var query = _context.CommentHistory.Where(c => c.CommentId == commentId);

                int totalItems = await query.CountAsync();

                var commentHistoryList = await query
                    .OrderByDescending(c => c.CreatedAt)
                    .Skip(pageInfo.Skip)
                    .Take(pageInfo.PageSize)
                    .ToListAsync();

                if (commentHistoryList == null)
                {
                    return new OperationResult
                    {
                        Success = false,
                        ErrorMessage = "Comment History not found."
                    };
                }

                List<CommentHistoryDto> commentHistoryDtoList = commentHistoryList.Adapt<List<CommentHistoryDto>>();

                return new OperationResult
                {
                    Success = true,
                    Data = new Dictionary<string, object>
                    {
                        { "content", commentHistoryDtoList },
                        { "pageInfo", new PageInfo
                            {
                                CurrentPage = pageInfo.CurrentPage,
                                PageSize = pageInfo.PageSize,
                                TotalItems = totalItems,
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
    }
}
