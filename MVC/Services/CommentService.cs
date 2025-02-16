using MVC.Models.ViewModels;
using MVC.Models;
using Common.Models;
using Common.Dto.Comment;
using MVC.Repositories;

namespace MVC.Services
{
    public class CommentService
    {
        private const string COMMENT_PREFIX = "Comment";
        private const string USER_PREFIX = "User";
        private readonly HttpClient _httpClient;
        private readonly ApiRepository _commonApiService;
        public CommentService(HttpClient httpClient, ApiRepository commonApiService)
        {
            _httpClient = httpClient;
            _commonApiService = commonApiService;
        }

        public async Task<ApiResponseOld<Comment>> GetCommentByIdAsync(int commentId)
        {
            if (commentId <= 0)
            {
                return new ApiResponseOld<Comment>();
            }

            string url = $"{COMMENT_PREFIX}/{commentId}";

            return await _commonApiService.GetApiResponseAsyncOld<Comment>(url);
        }

        public async Task<ApiResponseOld<Comment>> CreateCommentAsync(CreateCommentViewModel viewModel, string bearerToken)
        {
            if (viewModel == null)
            {
                return new ApiResponseOld<Comment>();
            }

            CreateCommentDto createcommentDto = new CreateCommentDto
            {
                Content = viewModel.Content,
                PostId = viewModel.PostId
            };

            return await _commonApiService.PostApiReponseAsync<Comment>($"{COMMENT_PREFIX}", createcommentDto, bearerToken);
        }

        public async Task<ApiResponseOld<Comment>> UpdateCommentByIdAsync(int commentId, UpdateCommentDto updateCommentDto, string bearerToken)
        {
            if (commentId <= 0)
            {
                return new ApiResponseOld<Comment>();
            }

            return await _commonApiService.PutAsync<Comment>($"{COMMENT_PREFIX}", updateCommentDto, bearerToken);
        }

        public async Task<ApiResponseOld<bool>> DeleteCommentAsync(int commentId, string bearerToken)
        {
            if (commentId <= 0)
            {
                return new ApiResponseOld<bool>();
            }

            return await _commonApiService.DeleteAsync<bool>($"{COMMENT_PREFIX}?commentId={commentId}", bearerToken);
        }

        public async Task<ApiResponseOld<Comment>> GetCommentsByUserIdAsync(string username, PageInfo pageInfo)
        {
            if (string.IsNullOrEmpty(username))
            {
                return new ApiResponseOld<Comment>();
            }

            string url = _commonApiService.StringFactory($"{USER_PREFIX}/{username}/comments", pageInfo.CurrentPage, pageInfo.PageSize);

            return await _commonApiService.GetApiResponseAsyncOld<Comment>(url);
        }
    }
}
