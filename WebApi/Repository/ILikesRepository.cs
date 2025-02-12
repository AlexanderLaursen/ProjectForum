namespace WebApi.Repository
{
    public interface ILikesRepository
    {
        public Task<bool> LikePostAsync(int postId, string userId);
    }
}