using MVC.Models;

namespace MVC.Services
{
    public class PostHistoryService
    {
        const string POST_HISTORY_PREFIX = "PostHistory";
        private readonly CommonApiService _commonApiService;

        public PostHistoryService(CommonApiService commonApiService)
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

            return await _commonApiService.GetApiResponseAsync<PostHistory>(url);

        }
    }
}