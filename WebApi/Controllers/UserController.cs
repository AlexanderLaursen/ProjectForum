using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using WebApi.Dto;
using WebApi.Dto.User;
using WebApi.Models;
using WebApi.Repository;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : Controller
    {
        private readonly ICommonRepository _commonRepository;
        private readonly IUserRepository _userRepository;
        private readonly BlobStorageService _blobStorageService;
        public UserController(ICommonRepository commonRepository, IUserRepository userRepository, BlobStorageService blobStorageService)
        {
            _commonRepository = commonRepository;
            _userRepository = userRepository;
            _blobStorageService = blobStorageService;
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

        [HttpGet("{username}")]
        public async Task<IActionResult> GetUserAsync(string username)
        
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest();
            }

            OperationResult result = await _userRepository.GetUserByUsernameAsync(username);
            if (!result.Success)
            {
                return NotFound();
            }

            return Ok(result.Data);
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

        //[HttpPost("/register/extend")]
        //public async Task<IActionResult> RegisterExtended(RegisterDto registerDto)
        //{
        //    if (registerDto == null)
        //    {
        //        return BadRequest();
        //    }

        //    OperationResult userOperation = await _userRepository.GetUserByUsernameAsync(registerDto.Email);

        //    if (!userOperation.IsSuccess)
        //    {
        //        return BadRequest();
        //    }

        //    if (userOperation.InternalData is not AppUser user)
        //    {
        //        return BadRequest();
        //    }



        //}
    }
}
