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

        public PostHistoryRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<OperationResult> GetPostHistoryByIdAsync(int postId, PageInfo pageInfo)
        {
            try
            {

                if (postId <= 0)
                {
                    return new OperationResult();
                }

                var query = _context.PostHistory
                    .Where(p => p.PostId == postId);

                int totalCount = await query.CountAsync();

                var result = await query
                    .OrderByDescending(p => p.CreatedAt)
                    .Skip(pageInfo.Skip)
                    .Take(pageInfo.PageSize)
                    .ToListAsync();

                if (result == null)
                {
                    return new OperationResult
                    {
                        Success = false,
                        ErrorMessage = "Post History not found."
                    };
                }

                List<PostHistoryDto> postHistoryDtoList = result.Adapt<List<PostHistoryDto>>();

                return new OperationResult
                {
                    Success = true,
                    Data = new Dictionary<string, object>
                {
                    { "content", postHistoryDtoList },
                    { "pageInfo", new PageInfo
                        {
                            CurrentPage = pageInfo.CurrentPage,
                            PageSize = pageInfo.PageSize,
                            TotalItems = totalCount,
                        }
                    }
                }
                };
            }
            catch
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = "An error occurred while fetching post history."
                };
            }
        }
    }
}
