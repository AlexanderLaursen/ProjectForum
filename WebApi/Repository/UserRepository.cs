using Mapster;
using Microsoft.AspNetCore.Identity;
using WebApi.Data;
using Common.Dto;
using WebApi.Models;
using WebApi.Repository.Interfaces;
using Common.Dto.User;
using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly DataContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(UserManager<AppUser> userManager, DataContext context, ILogger<UserRepository> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        public async Task<Result<AppUser>> GetAppUserAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user != null)
                {
                    return Result<AppUser>.Success(user);
                }

                return Result<AppUser>.NotFound("User not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the user.");
                return Result<AppUser>.Failure("Unexpected error while getting user.");
            }
        }

        public async Task<Result<UserDto>> GetUserAsync(string userName)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user == null)
                {
                    return Result<UserDto>.NotFound("User not found.");
                }

                return Result<UserDto>.Success(user.Adapt<UserDto>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the user.");
                return Result<UserDto>.Failure("Unexpected error while getting user.");
            }
        }

        public async Task<Result<UserDto>> UpdateUserAsync(AppUser user)
        {
            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();

                return Result<UserDto>.Success(user.Adapt<UserDto>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the user.");
                return Result<UserDto>.Failure("Unexpected error while updating user.");
            }
        }
    }
}
