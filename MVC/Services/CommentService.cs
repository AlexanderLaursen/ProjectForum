using MVC.Models.ViewModels;
using MVC.Models;
using MVC.Models.Dto;

namespace MVC.Services
{
    public class CommentService
    {
        private const string COMMENT_PREFIX = "Comment";
        private readonly HttpClient _httpClient;
        private readonly CommonApiService _commonApiService;
        public CommentService(HttpClient httpClient, CommonApiService commonApiService)
        {
            _httpClient = httpClient;
            _commonApiService = commonApiService;
        }

        public async Task<ApiResponse<Comment>> GetCommentByIdAsync(int commentId)
        {
            if (commentId <= 0)
            {
                return new ApiResponse<Comment>();
            }

            string url = $"{COMMENT_PREFIX}/{commentId}";

            return await _commonApiService.GetApiResponseAsync<Comment>(url);
        }

        public async Task<ApiResponse<Comment>> CreateCommentAsync(CreateCommentViewModel viewModel, string bearerToken)
        {
            if (viewModel == null)
            {
                return new ApiResponse<Comment>();
            }

            CreateCommentDto createcommentDto = new CreateCommentDto
            {
                Content = viewModel.Content,
                PostId = viewModel.PostId
            };

            return await _commonApiService.PostApiReponseAsync<Comment>($"{COMMENT_PREFIX}", createcommentDto, bearerToken);
        }

        public async Task<ApiResponse<Comment>> UpdateCommentByIdAsync(int commentId, UpdateCommentDto updateCommentDto, string bearerToken)
        {
            if (commentId <= 0)
            {
                return new ApiResponse<Comment>();
            }

            return await _commonApiService.PutAsync<Comment>($"{COMMENT_PREFIX}", updateCommentDto, bearerToken);
        }

        public async Task<ApiResponse<bool>> DeleteCommentAsync(int commentId, string bearerToken)
        {
            if (commentId <= 0)
            {
                return new ApiResponse<bool>();
            }

            return await _commonApiService.DeleteAsync<bool>($"{COMMENT_PREFIX}?commentId={commentId}", bearerToken);
        }
    }
}
