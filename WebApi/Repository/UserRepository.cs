using Mapster;
using Microsoft.AspNetCore.Identity;
using WebApi.Data;
using Common.Dto;
using WebApi.Models;
using WebApi.Repository.Interfaces;
using Common.Dto.User;

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
            List<UserDto> userList = [userDto];

            return new OperationResult
            {
                Success = true,
                Data = new Dictionary<string, object>
                    {
                        { "content", userList }
                    },
                InternalData = user
            };

        }

        public Task<OperationResult> UpdateUserAsync(AppUser user)
        {
            if (user == null)
            {
                return Task.FromResult(new OperationResult
                {
                    Success = false,
                    ErrorMessage = "Invalid user."
                });
            }

            try
            {
                _context.Update(user);
                _context.SaveChangesAsync();

                return Task.FromResult(new OperationResult
                {
                    Success = true,
                    InternalData = user
                });
            }
            catch (Exception)
            {
                return Task.FromResult(new OperationResult
                {
                    Success = false,
                    ErrorMessage = "An error occurred while updating the user."
                });
            }
        }
    }
}
