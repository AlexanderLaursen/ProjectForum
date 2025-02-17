using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Repository.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using Common.Dto.User;
using Common.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ICommonRepository _commonRepository;
        private readonly IUserRepository _userRepository;
        private readonly BlobStorageService _blobStorageService;
        private readonly ILogger<UserController> _logger;
        public UserController(ICommonRepository commonRepository, IUserRepository userRepository, BlobStorageService blobStorageService, ILogger<UserController> logger)
        {
            _commonRepository = commonRepository;
            _userRepository = userRepository;
            _blobStorageService = blobStorageService;
            _logger = logger;
        }

        [HttpGet("{username}/id")]
        public IActionResult GetUserIdByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest();
            }

            string? userId = _commonRepository.GetUserIdByUsernameAsync(username).Result;

            if (string.IsNullOrWhiteSpace(userId))
            {
                return NotFound();
            }

            Dictionary<string, object> data = new()
            {
                 { "content", new List<string> { userId } }
            };

            return Ok(data);
        }

        [HttpGet("/api/v2/username/id")]
        
        [HttpGet("/api/v2/{username}")]
        public async Task<IActionResult> GetUserAsync(string username)
        
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest();
            }

            Result<UserDto> result = await _userRepository.GetUserByUsernameAsync(username);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            _logger.LogError("Error occurred while getting user.");
            return HandleErrors<UserDto>(result);
        }

        [Authorize]
        [HttpPost("upload-profile-picture")]
        public async Task<IActionResult> UploadProfilePicture(IFormFile file, string username)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            OperationResult userOperation = await _userRepository.GetUserByUsernameAsync(username);

            if (userOperation == null)
            {
                return Unauthorized("Invalid user credentials.");
            }

            var user = userOperation.InternalData as AppUser;

            if (user == null)
            {
                return Unauthorized("Invalid user credentials.");
            }

            using var stream = file.OpenReadStream();

            string fileName = $"original/{user.Id}";
            string filePath = await _blobStorageService.UploadFileAsync(stream, fileName);

            string newUrl = filePath.Replace("original", "resized");

            user.SmProfilePicture = newUrl + "_50.jpg";
            user.MdProfilePicture = newUrl + "_100.jpg";
            user.LgProfilePicture = newUrl + "_300.jpg";

            await _userRepository.UpdateUserAsync(user);

            return Ok();
        }
    }
}
