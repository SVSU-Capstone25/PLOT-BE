/*
This should just need to be added to the AuthController in the Auth branch
*/

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Plot.Controllers
{

    [ApiController]
    [Route("/Auth")]
    public class AuthController : ControllerBase
    {

        [HttpGet("me")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized(new { Message = "User not logged in" });
            }

            return Ok(new { Email = userEmail, Role = userRole });
        }
    }
}