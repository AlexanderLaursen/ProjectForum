using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
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
        public UserController(ICommonRepository commonRepository, IUserRepository userRepository)
        {
            _commonRepository = commonRepository;
            _userRepository = userRepository;
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
    }
}
