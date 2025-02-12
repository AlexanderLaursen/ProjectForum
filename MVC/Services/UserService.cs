using MVC.Models;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MVC.Services
{
    public class UserService
    {
        private const string USER_PREDIX = "User";
        private readonly CommonApiService _commonApiService;
        private readonly HttpClient _httpClient;
        public UserService(CommonApiService commonApiService, HttpClient httpClient)
        {
            _commonApiService = commonApiService;
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<AppUser>> GetUserByUsernameAsync (string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return new ApiResponse<AppUser>();
            }

            string url = _commonApiService.StringFactory($"{USER_PREDIX}/{username}");

            return await _commonApiService.GetApiResponseAsync<AppUser>(url);
        }

        public async Task<HttpResponseMessage> UploadPictureAsync(IFormFile file, string username, string bearerToken)
        {
            if (file == null || string.IsNullOrEmpty(username))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            using var content = new MultipartFormDataContent();
            using var fileStream = file.OpenReadStream();
            using var streamContent = new StreamContent(fileStream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            content.Add(streamContent, "file", file.FileName);

            var response = await _httpClient.PostAsync($"https://localhost:7052/api/v1/User/upload-profile-picture?username={username}", content);
            return response;
        }
    }
}