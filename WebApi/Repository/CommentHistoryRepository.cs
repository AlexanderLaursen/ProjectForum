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
        private readonly ILogger<CommentHistoryRepository> _logger;

        public CommentHistoryRepository(DataContext context, ILogger<CommentHistoryRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<CommentHistoriesDto>> GetCommentHistoryAsync(int commentId, PageInfo pageInfo)
        {
            try
            {
                var query = _context.CommentHistory.Where(c => c.CommentId == commentId);

                int totalItems = await query.CountAsync();

                var commentHistoryList = await query
                    .Select(ch => new CommentHistoryDto
                    {
                        Id = ch.Id,
                        CommentId = ch.CommentId,
                        Content = ch.Content,
                        CreatedAt = ch.CreatedAt,
                        UserId = ch.UserId
                    })
                    .OrderByDescending(c => c.CreatedAt)
                    .Skip(pageInfo.Skip)
                    .Take(pageInfo.PageSize)
                    .ToListAsync();

                if (commentHistoryList.Count == 0)
                {
                    return Result<CommentHistoriesDto>.NotFound();
                }

                CommentHistoriesDto commentHistoriesDto = new CommentHistoriesDto
                {
                    CommentHistories = commentHistoryList,
                    PageInfo = new PageInfo(pageInfo.CurrentPage, pageInfo.PageSize, totalItems),
                };

                return Result<CommentHistoriesDto>.Success(commentHistoriesDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting comment history.");
                return Result<CommentHistoriesDto>.Failure("Unknown error.");
            }

        }
    }
}
