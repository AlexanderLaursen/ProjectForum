using Common.Enums;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public abstract class ControllerBase : Controller
    {
        public IActionResult HandleErrors<T>(Result<T> result)
        {
            switch (result.Status)
            {
                case ResultStatus.NotFound:
                    return NotFound();
                case ResultStatus.InvalidInput:
                    return BadRequest(result.ErrorMessage);
                case ResultStatus.Unauthorized:
                    return Unauthorized();
                case ResultStatus.Error:
                default:
                    return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error occurred.");
            }
        }
    }
}
