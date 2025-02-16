﻿using Common.Dto.Post;
using Common.Enums;
using MVC.Models;
using MVC.Models.ViewModels;
using Common.Models;
using Mapster;
using MVC.Repositories;

namespace MVC.Services
{
    public class PostService
    {
        private const string CATEGORY_PREFIX = "Category";
        private const string POST_PREFIX = "Post";
        private const string USER_PREFIX = "User";
        private readonly HttpClient _httpClient;
        private readonly ApiRepository _commonApiService;

        public PostService(HttpClient httpClient, ApiRepository commonApiService)
        {
            _httpClient = httpClient;
            _commonApiService = commonApiService;
        }

        public async Task<ApiResponse<PostDetails>> GetPostDetails(int id, PageInfo pageInfo, string bearer)
        {
            string url = _commonApiService.StringFactory($"{POST_PREFIX}/{id}/details", pageInfo.CurrentPage, pageInfo.PageSize);

            var resultDto = await _commonApiService.GetAuthAsync<PostDetailsDto>(url, bearer);

            if (resultDto.Content.PostDto == null)
            {
                return ApiResponse<PostDetails>.Fail();
            }

            PostDetails postDetails = new()
            {
                Post = resultDto.Content.PostDto.Adapt<Post>(),
                Comments = resultDto.Content.CommentsDto.Adapt<List<Comment>>(),
                PageInfo = resultDto.PageInfo
            };

            return ApiResponse<PostDetails>.Success(postDetails);
        }

        public async Task<ApiResponseOld<Post>> GetPostsByCategoryIdAsync(int categoryId, PageInfo pageInfo, SortDirection sortDirection, SortBy sortBy)
        {
            if (categoryId <= 0)
            {
                return new ApiResponseOld<Post>();
            }

            string url = _commonApiService.StringFactory($"{CATEGORY_PREFIX}/{categoryId}/posts", pageInfo.CurrentPage, pageInfo.PageSize);

            url += $"&sortDirection={sortDirection}&sortBy={sortBy}";

            return await _commonApiService.GetApiResponseAsyncOld<Post>(url);
        }

        public async Task<ApiResponseOld<Post>> GetPostByIdAsync(int id, PageInfo pageInfo)
        {
            if (id <= 0)
            {
                return new ApiResponseOld<Post>();
            }

            string url = _commonApiService.StringFactory($"{POST_PREFIX}/{id}", pageInfo.CurrentPage, pageInfo.PageSize);

            return await _commonApiService.GetApiResponseAsyncOld<Post>(url);
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

            return await _commonApiService.GetApiResponseAsyncOld<Post>(url);
        }
    }

    public class Nonsense
    {
        public string No { get; set; }
    }
}
