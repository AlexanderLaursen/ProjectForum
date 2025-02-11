using MVC.Models;

namespace MVC.Services
{
    public class UserService
    {
        private const string USER_PREDIX = "User";
        private readonly CommonApiService _commonApiService;
        public UserService(CommonApiService commonApiService)
        {
            _commonApiService = commonApiService;
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

    }
}