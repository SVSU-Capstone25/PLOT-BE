using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Plot.Context;
using Plot.Data.Models.PasswordReset;
using Plot.Services;
using Plot.Data.Models.User;

namespace Plot.Controllers
{
    [Route("api/password-reset")]
    [ApiController]
    public class PasswordResetController : ControllerBase
    {
        private const string RESET_LINK_TEMPLATE = "http://localhost:500/reset-password.html?token=";//"https://www.plot.com/reset-password?token=";
        
        private readonly EmailService _emailService;
        
        private readonly PlotContext _context;
        private readonly TokenService _tokenService;

         public PasswordResetController(EmailService emailService, 
         PlotContext context, TokenService tokenService)  
        {
            _emailService = emailService;
            _context = context;
            _tokenService = tokenService;
        }
        

        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] RequestPasswordReset receivedEmailRequest)
        {
            var receivedEmail = receivedEmailRequest.EmailAddress;
            
            

            if (receivedEmail == null)
            {
                return BadRequest("Email address not found.");
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == receivedEmail);

            if (user == null)
            {
                return BadRequest("Email address not found.");
            }

            string resetToken = _tokenService.GenerateToken(receivedEmail);


            string resetLink = RESET_LINK_TEMPLATE + resetToken;
            string name = user.FirstName ?? string.Empty;
            string email = user.Email ?? string.Empty;

            await _emailService.SendPasswordResetEmailAsync(name, email, resetLink);
        
            return Ok("Email sent");
        }


        [HttpPost("reset-password")]
        public IActionResult ResetPassword([FromBody] ResetPassword receivedResetPassword)
        {
            var token = receivedResetPassword.Token;
            var newPassword = receivedResetPassword.NewPassword;

            if (token == null || newPassword == null)
            {
                return BadRequest("Token or password hash not found.");
            }

            var email = _tokenService.ValidateToken(token);

            if (email == null)
            {
                return BadRequest("Invalid token.");
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                return BadRequest("User not found.");
            }

            PasswordHasher<User> hasher = new PasswordHasher<User>();

            var hashedPassword = hasher.HashPassword(user, newPassword);

            user.Password = hashedPassword;

            _context.SaveChanges();

            return Ok("Password reset successful.");
        }
        
    }
}