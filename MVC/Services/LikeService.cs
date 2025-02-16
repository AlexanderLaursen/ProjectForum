using MVC.Models;

namespace MVC.Services
{
    public class LikeService : CommonApiService
    {
        public LikeService(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<ApiResponseOld<object>> LikePostAsync (int id, string bearerToken)
        {
            if (id <= 0 || string.IsNullOrEmpty(bearerToken))
            {
                return ApiResponseOld<object>.Fail();
            }

            string url = StringFactory($"Likes/post/{id}");

            var response = await PostApiReponseAsync<ApiResponseOld<object>>(url, new {}, bearerToken);

            if (response.IsSuccess)
            {
                return ApiResponseOld<object>.Success();
            }

            return ApiResponseOld<object>.Fail();
        }
        public async Task<ApiResponseOld<object>> LikeCommentAsync(int id, string bearerToken)
        {
            if (id <= 0 || string.IsNullOrEmpty(bearerToken))
            {
                return ApiResponseOld<object>.Fail();
            }

            string url = StringFactory($"Likes/comment/{id}");

            var response = await PostApiReponseAsync<ApiResponseOld<object>>(url, new { }, bearerToken);

            if (response.IsSuccess)
            {
                return ApiResponseOld<object>.Success();
            }

            return ApiResponseOld<object>.Fail();
        }

        public async Task<ApiResponse<object>> DislikePostAsync(int id, string bearerToken)
        {
            if (id <= 0 || string.IsNullOrEmpty(bearerToken))
            {
                return ApiResponse<object>.Fail();
            }

            string url = StringFactory($"Likes/post/{id}");

            var response = await DeleteAsync<ApiResponse<object>>(url, bearerToken);
            if (response.IsSuccess)
            {
                return ApiResponse<object>.Success();
            }

            return ApiResponse<object>.Fail();
        }

        public async Task<ApiResponse<object>> DislikeCommentAsync(int id, string bearerToken)
        {
            if (id <= 0 || string.IsNullOrEmpty(bearerToken))
            {
                return ApiResponse<object>.Fail();
            }

            string url = StringFactory($"Likes/comment/{id}");

            var response = await DeleteAsync<ApiResponse<object>>(url, bearerToken);
            if (response.IsSuccess)
            {
                return ApiResponse<object>.Success();
            }

            return ApiResponse<object>.Fail();
        }
    }
}
