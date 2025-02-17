using Mapster;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using Common.Dto.PostHistory;
using WebApi.Models;
using WebApi.Repository.Interfaces;
using Common.Models;

namespace WebApi.Repository
{
    public class PostHistoryRepository : IPostHistoryRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<PostHistoryRepository> _logger;

        public PostHistoryRepository(DataContext context, ILogger<PostHistoryRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<PostHistoriesDto>> GetPostHistoryAsync(int postId, PageInfo pageInfo)
        {
            try
            {
                var query = _context.PostHistory
                    .Where(p => p.PostId == postId);

                int totalCount = await query.CountAsync();

                var postHistoryResult = await query
                    .Select(ph => new PostHistoryDto
                    {
                        Id = ph.Id,
                        PostId = ph.PostId,
                        Title = ph.Title,
                        Content = ph.Content,
                        CreatedAt = ph.CreatedAt,
                        UserId = ph.UserId
                    })
                    .OrderByDescending(p => p.CreatedAt)
                    .Skip(pageInfo.Skip)
                    .Take(pageInfo.PageSize)
                    .ToListAsync();

                if (postHistoryResult.Count == 0)
                {
                    return Result<PostHistoriesDto>.NotFound();
                }

                PostHistoriesDto postHistoriesDto = new PostHistoriesDto
                {
                    PostHistories = postHistoryResult,
                    PageInfo = new PageInfo(pageInfo.CurrentPage, pageInfo.PageSize, totalCount),
                };

                return Result<PostHistoriesDto>.Success(postHistoriesDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting post history.");
                return Result<PostHistoriesDto>.Failure("Unkown error while fetching PostHistory from database.");
            }
        }
    }
}
