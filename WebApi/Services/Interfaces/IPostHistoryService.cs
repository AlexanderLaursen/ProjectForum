using Common.Dto.PostHistory;
using Common.Models;

namespace WebApi.Services.Interfaces
{
    public interface IPostHistoryService
    {
        public Task<Result<PostHistoriesDto>> GetPostHistoryAsync(int postId, PageInfo pageInfo);
    }
}
