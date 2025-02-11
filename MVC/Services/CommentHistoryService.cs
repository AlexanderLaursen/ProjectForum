using MVC.Models;

namespace MVC.Services
{
    public class CommentHistoryService
    {
        const string COMMENT_HISTORY_PREFIX = "CommentHistory";
        private readonly CommonApiService _commonApiService;

        public CommentHistoryService(CommonApiService commonApiService)
        {
            _commonApiService = commonApiService;
        }

        public async Task<ApiResponse<CommentHistory>> GetCommentHistoryByIdAsync(int commentId, PageInfo pageInfo)
        {
            if (commentId <= 0)
            {
                return new ApiResponse<CommentHistory>();
            }

            string url = _commonApiService.StringFactory($"{COMMENT_HISTORY_PREFIX}/{commentId}", pageInfo.CurrentPage, pageInfo.PageSize);

            return await _commonApiService.GetApiResponseAsync<CommentHistory>(url);
        }
    }
}
