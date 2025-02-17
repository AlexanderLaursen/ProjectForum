using Common.Models;

namespace MVC.Repositories.Interfaces
{
    public interface IApiRepository
    {
        public Task<Result<T>> GetAsync<T>(string url, string? bearerToken = default);

        public Task<Result<T>> PostAsync<T>(string url, object data, string? bearerToken = default);
    }
}