using MVC.Models;
using Common.Models;
using MVC.Repositories;

namespace MVC.Services
{
    public class CommentHistoryService
    {
        const string COMMENT_HISTORY_PREFIX = "CommentHistory";
        private readonly ApiRepository _commonApiService;

        public CommentHistoryService(ApiRepository commonApiService)
        {
            _commonApiService = commonApiService;
        }

        public async Task<ApiResponseOld<CommentHistory>> GetCommentHistoryByIdAsync(int commentId, PageInfo pageInfo)
        {
            if (commentId <= 0)
            {
                return new ApiResponseOld<CommentHistory>();
            }

            string url = _commonApiService.StringFactory($"{COMMENT_HISTORY_PREFIX}/{commentId}", pageInfo.CurrentPage, pageInfo.PageSize);

            return await _commonApiService.GetApiResponseAsyncOld<CommentHistory>(url);
        }
    }
}
