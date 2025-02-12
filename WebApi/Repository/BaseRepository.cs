using WebApi.Data;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Repository
{
    public class BaseRepository
    {
        protected readonly DataContext _context;

        public BaseRepository(DataContext context)
        {
            _context = context;
        }

        public async Task IncrementViewCountAsync(int postId)
        {
            await _context.Database.ExecuteSqlRawAsync($"CALL increment_post_view_count({postId});");
        }
    }
}
