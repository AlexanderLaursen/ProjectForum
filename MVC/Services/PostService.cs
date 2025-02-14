using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Models.Dto;
using MVC.Models.ViewModels;
using System.Net.Http.Headers;
using System.Text;

namespace MVC.Services
{
    public class PostService
    {
        private const string CATEGORY_PREFIX = "Category";
        private const string POST_PREFIX = "Post";
        private const string USER_PREFIX = "User";
        private readonly HttpClient _httpClient;
        private readonly CommonApiService _commonApiService;

        public PostService(HttpClient httpClient, CommonApiService commonApiService)
        {
            _httpClient = httpClient;
            _commonApiService = commonApiService;
        }

        public async Task Test(string bearer)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearer);
            var result = await _httpClient.GetAsync("https://localhost:7052/api/v1/Post/test2");

        }

        public async Task<ApiResponse<PostDetailsDto>> GetPostDetails(int id, PageInfo pageInfo, string bearer)
        {
            string url = _commonApiService.StringFactory($"{POST_PREFIX}/{id}/details", pageInfo.CurrentPage, pageInfo.PageSize);

            return await _commonApiService.GetAuthAsync<PostDetailsDto>(url, bearer);
        }

        public async Task<ApiResponseOld<Post>> GetPostsByCategoryIdAsync(int categoryId, PageInfo pageInfo)
        {
            if (categoryId <= 0)
            {
                return new ApiResponseOld<Post>();
            }

            string url = _commonApiService.StringFactory($"{CATEGORY_PREFIX}/{categoryId}/posts", pageInfo.CurrentPage, pageInfo.PageSize);

            return await _commonApiService.GetApiResponseAsync<Post>(url);
        }

        public async Task<ApiResponseOld<Post>> GetPostByIdAsync(int id, PageInfo pageInfo)
        {
            if (id <= 0)
            {
                return new ApiResponseOld<Post>();
            }

            string url = _commonApiService.StringFactory($"{POST_PREFIX}/{id}", pageInfo.CurrentPage, pageInfo.PageSize);

            return await _commonApiService.GetApiResponseAsync<Post>(url);
        }

        public async Task<ApiResponseOld<Post>> CreatePostAsync(CreatePostViewModel viewModel, string bearerToken)
        {
            if (viewModel == null)
            {
                return new ApiResponseOld<Post>();
            }

            CreatePostDto createPostDto = new CreatePostDto
            {
                Title = viewModel.Title,
                Content = viewModel.Content,
                CategoryId = viewModel.CategoryId
            };

            return await _commonApiService.PostApiReponseAsync<Post>($"{POST_PREFIX}", createPostDto, bearerToken);
        }

        public async Task<ApiResponseOld<bool>> DeletePostAsync(int id, string bearerToken)
        {
            if (id <= 0)
            {
                return new ApiResponseOld<bool>();
            }

            return await _commonApiService.DeleteAsync<bool>($"{POST_PREFIX}?postId={id}", bearerToken);
        }

        public async Task<ApiResponseOld<Post>> UpdatePostAsync(UpdatePostViewModel viewModel, string bearerToken)
        {
            if (viewModel == null)
            {
                return new ApiResponseOld<Post>();
            }

            UpdatePostDto updatePostDto = new UpdatePostDto
            {
                Title = viewModel.Title,
                Content = viewModel.Content,
                PostId = viewModel.PostId,
                CategoryId = viewModel.CategoryId
            };


            return await _commonApiService.PutAsync<Post>($"{POST_PREFIX}", updatePostDto, bearerToken);
        }

        public async Task<ApiResponseOld<Post>> GetPostsByUserIdAsync(string username, PageInfo pageInfo)
        {
            if (string.IsNullOrEmpty(username))
            {
                return new ApiResponseOld<Post>();
            }

            string url = _commonApiService.StringFactory($"{USER_PREFIX}/{username}/posts", pageInfo.CurrentPage, pageInfo.PageSize);

            return await _commonApiService.GetApiResponseAsync<Post>(url);
        }
    }

    public class Nonsense
    {
        public string No { get; set; }
    }
}
