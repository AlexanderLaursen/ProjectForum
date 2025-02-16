namespace WebApi.Repository.Interfaces
{
    public interface ICommonRepository
    {
        public Task<string?> GetUserIdByUsernameAsync(string username);
    }
}