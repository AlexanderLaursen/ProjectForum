using Common.Models;
using MVC.Models;

namespace MVC.Services.Interfaces
{
    public interface IPostHistoryApiService
    {
        public Task<Result<PostHistories>> GetPostHistoryAsync(int postId, PageInfo pageInfo);
    }
}