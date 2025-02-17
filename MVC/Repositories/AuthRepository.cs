using Common.Dto.User;
using Common.Models;
using MVC.Models;
using MVC.Repositories.Interfaces;

namespace MVC.Repositories
{
    public class AuthRepository : ApiRepository, IAuthRepository
    {
        public AuthRepository(HttpClient httpClient, ILogger<ApiRepository> logger) : base(httpClient, logger)
        {
        }

        public async Task<Result<BearerToken>> LoginAsync(object data, string? bearerToken = default)
        {
            string url = "https://localhost:7052/login";

            return await PostAsync<BearerToken>(url, data);
        }

        public async Task<Result<UserDto>> GetUserAsync(string userName)
        {
            string url = UrlFactory($"user/{userName}");
            return await GetAsync<UserDto>(url);
        }
    }
}
