namespace WebApi.Repository
{
    public interface ICommonRepository
    {
        public Task<string?> GetUserIdByUsernameAsync(string username);
    }
}