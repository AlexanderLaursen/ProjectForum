using WebApi.Models;
using Common.Models;
using Common.Dto.PostHistory;

namespace WebApi.Repository.Interfaces
{
    public interface IPostHistoryRepository
    {
        public Task<Result<PostHistoriesDto>> GetPostHistoryAsync(int postId, PageInfo pageInfo);
    }
}
