//using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Plot.Model;   
using Plot.Services;

[Route("api/[RequestPasswordReset]")]
[ApiController]
public class RequestPasswordResetController : ControllerBase
{

    public RequestPasswordResetController()
    {
    }

    [HttpPost("request-password-reset")]
    public async Task<IActionResult> RequestPasswordReset([FromBody] RequestPasswordResetModel requestPasswordResetModel)
    {
        PasswordResetService passwordResetService = new PasswordResetService();

        bool isEmailValid = passwordResetService.IsEmailValid(requestPasswordResetModel.EmailAddress);

        if (!isEmailValid)
        {
            return BadRequest("Invalid email address.");
        }

        string email = requestPasswordResetModel.EmailAddress;
        string name = passwordResetService.FindNameByEmail(email);

        if (name == null)
        {
            return BadRequest("Email address not found.");
        }

        UserManager<IdentityUser> userManager = new UserManager<IdentityUser>();
        userManager.GeneratePasswordResetTokenAsync();
        string resetToken = passwordResetService.GenerateResetToken();

        string resetLink = "https://www.plot.com/reset-password?token=" + resetToken;

        




    }
}
