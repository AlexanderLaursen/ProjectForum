using Microsoft.AspNetCore.Authorization;
using WebApi.Data;

namespace WebApi.Repository
{
    public class LikesRepository : BaseRepository, ILikesRepository
    {
        public LikesRepository(DataContext context) : base(context)
        {
        }

        public async Task<bool> LikePostAsync(int postId, string userId)
        {
            if (postId <= 0 || string.IsNullOrEmpty(userId))
            {
                return false;
            }

            return false;
        }
    }
}
