using Mapster;
using Microsoft.AspNetCore.Identity;
using WebApi.Data;
using WebApi.Dto;
using WebApi.Models;

namespace WebApi.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly DataContext _context;

        public UserRepository(UserManager<AppUser> userManager, DataContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<OperationResult> GetUserByUsernameAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = "Invalid username."
                };
            }

            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return new OperationResult
                {
                    Success = false,
                    ErrorMessage = "User not found."
                };
            }

            UserDto userDto = user.Adapt<UserDto>();

            return new OperationResult
            {
                Success = true,
                Data = new Dictionary<string, object>
                    {
                        { "user", userDto }
                    }
            };

        }
    }
}
