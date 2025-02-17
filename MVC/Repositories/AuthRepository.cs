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
            string url = UrlFactory(LOGIN);

            return await PostAsync<BearerToken>(url, data);
        }
    }
}
