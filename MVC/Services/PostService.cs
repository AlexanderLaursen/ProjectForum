﻿using MVC.Models;
using MVC.Models.Dto;
using MVC.Models.ViewModels;
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

        public async Task<ApiResponse<Post>> GetPostsByCategoryIdAsync(int categoryId, PageInfo pageInfo)
        {
            if (categoryId <= 0)
            {
                return new ApiResponse<Post>();
            }

            string url = _commonApiService.StringFactory($"{CATEGORY_PREFIX}/{categoryId}/posts", pageInfo.CurrentPage, pageInfo.PageSize);

            return await _commonApiService.GetApiResponseAsync<Post>(url);
        }

        public async Task<ApiResponse<Post>> GetPostByIdAsync(int id, PageInfo pageInfo)
        {
            if (id <= 0)
            {
                return new ApiResponse<Post>();
            }

            string url = _commonApiService.StringFactory($"{POST_PREFIX}/{id}", pageInfo.CurrentPage, pageInfo.PageSize);

            return await _commonApiService.GetApiResponseAsync<Post>(url);
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

        public async Task<ApiResponse<bool>> DeletePostAsync(int id, string bearerToken)
        {
            if (id <= 0)
            {
                return new ApiResponse<bool>();
            }

            return await _commonApiService.DeleteAsync<bool>($"{POST_PREFIX}?postId={id}", bearerToken);
        }

        public async Task<ApiResponse<Post>> UpdatePostAsync(UpdatePostViewModel viewModel, string bearerToken)
        {
            if (viewModel == null)
            {
                return new ApiResponse<Post>();
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

        public async Task<ApiResponse<Post>> GetPostsByUserIdAsync(string username, PageInfo pageInfo)
        {
            if (string.IsNullOrEmpty(username))
            {
                return new ApiResponse<Post>();
            }

            string url = _commonApiService.StringFactory($"{USER_PREFIX}/{username}/posts", pageInfo.CurrentPage, pageInfo.PageSize);

            return await _commonApiService.GetApiResponseAsync<Post>(url);
        }
    }
}
