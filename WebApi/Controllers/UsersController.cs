using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Repository.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using Common.Dto.User;
using Common.Models;
using WebApi.Services.Interfaces;
using System.Security.Claims;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v2/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly BlobStorageService _blobStorageService;
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;
        public UsersController(BlobStorageService blobStorageService, ILogger<UsersController> logger, IUserService userService)
        {
            _blobStorageService = blobStorageService;
            _userService = userService;
            _logger = logger;
        }
        
        [HttpGet("{username}")]
        public async Task<IActionResult> GetUserAsync(string username)
        
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest();
            }

            Result<UserDto> result = await _userService.GetUserAsync(username);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            _logger.LogError("Error occurred while getting user.");
            return HandleErrors<UserDto>(result);
        }

        [Authorize]
        [HttpPost("upload-profile-picture")]
        public async Task<IActionResult> UploadProfilePicture(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            Result<AppUser> userResult = await _userService.GetAppUserAsync(userId);

            if (!userResult.IsSuccess || userResult.Value == null)
            {
                return BadRequest();
            }

            AppUser user = userResult.Value;

            using var stream = file.OpenReadStream();

            string fileName = $"original/{userId}";
            string filePath = await _blobStorageService.UploadFileAsync(stream, fileName);

            string newUrl = filePath.Replace("original", "resized");

            user.SmProfilePicture = newUrl + "_50.jpg";
            user.MdProfilePicture = newUrl + "_100.jpg";
            user.LgProfilePicture = newUrl + "_300.jpg";

            await _userService.UpdateUserAsync(user);

            return Ok();
        }
    }
}
