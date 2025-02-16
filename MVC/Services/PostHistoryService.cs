using MVC.Models;
using Common.Models;
using MVC.Repositories;

namespace MVC.Services
{
    public class PostHistoryService
    {
        const string POST_HISTORY_PREFIX = "PostHistory";
        private readonly ApiRepository _commonApiService;

        public PostHistoryService(ApiRepository commonApiService)
        {
            _commonApiService = commonApiService;
        }

        public async Task<ApiResponseOld<PostHistory>> GetPostHistoryByIdAsync(int postId, PageInfo pageInfo)
        {
            if (postId <= 0)
            {
                return new ApiResponseOld<PostHistory>();
            }

            string url = _commonApiService.StringFactory($"{POST_HISTORY_PREFIX}/{postId}", pageInfo.CurrentPage, pageInfo.PageSize);

            return await _commonApiService.GetApiResponseAsyncOld<PostHistory>(url);

        }
    }
}