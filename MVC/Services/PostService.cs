using MVC.Models;
using MVC.Models.Dto;
using MVC.Models.ViewModels;

namespace MVC.Services
{
    public class PostService
    {
        private const string CATEGORY_PREFIX = "Category";
        private const string POST_PREFIX = "Post";
        private readonly HttpClient _httpClient;
        private readonly CommonApiService _commonApiService;
        public PostService(HttpClient httpClient, CommonApiService commonApiService)
        {
            _httpClient = httpClient;
            _commonApiService = commonApiService;
        }

        public async Task<ApiResponse<Post>> GetPostsByCategoryId(int categoryId)
        {
            if (categoryId <= 0)
            {
                return new ApiResponse<Post>();
            }

            return await _commonApiService.GetApiResponseAsync<Post>($"{CATEGORY_PREFIX}/{categoryId}/posts");
        }

        public async Task<ApiResponse<Post>> GetPostByIdAsync(int id)
        {
            if (id <= 0)
            {
                return new ApiResponse<Post>();
            }

            return await _commonApiService.GetApiResponseAsync<Post>($"{POST_PREFIX}/{id}");
        }

        public async Task<ApiResponse<Post>> CreatePostAsync(CreatePostViewModel viewModel, string bearerToken)
        {
            if (viewModel == null)
            {
                return new ApiResponse<Post>();
            }

            CreatePostDto createPostDto = new CreatePostDto
            {
                Title = viewModel.Title,
                Content = viewModel.Content,
                CategoryId = viewModel.CategoryId
            };

            return await _commonApiService.PostApiReponseAsync<Post>($"{POST_PREFIX}", createPostDto, bearerToken);
        }
    }
}
