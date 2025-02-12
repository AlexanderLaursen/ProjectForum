namespace WebApi.Repository
{
    public interface IBaseRepository
    {
        public Task IncrementViewCountAsync(int postId);
    }
}
