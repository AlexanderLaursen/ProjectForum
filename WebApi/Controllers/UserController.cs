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
        public UserController(ICommonRepository commonRepository)
        {
            _commonRepository = commonRepository;
        }

        [HttpGet("{username}")]
        public IActionResult GetUserByUsername(string username)
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
    }
}
