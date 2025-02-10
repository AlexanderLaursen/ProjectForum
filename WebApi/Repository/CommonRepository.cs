using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Repository
{
    public class CommonRepository : ICommonRepository
    {
        private readonly DataContext _context;
        public CommonRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<string?> GetUserIdByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return string.Empty;
            }

            string? userId = string.Empty;

            try
            {
                userId = await _context.Users
                    .Where(u => u.NormalizedUserName == username.ToUpper())
                    .Select(u => u.Id)
                    .FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                // TODO: Implement logging
            }
            
            return userId;
        }
    }
}
