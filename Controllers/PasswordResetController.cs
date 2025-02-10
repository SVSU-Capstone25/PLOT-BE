using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Plot.Context;
using Plot.Data.Models.PasswordReset;
using Plot.Services;
using Plot.Data.Models.User;


namespace Plot.Controllers
{
    /// <summary>
    /// Filename: PasswordResetController.cs
    /// Part of Project: PLOT (can rename later)
    ///
    /// File Purpose:
    /// This file contains the implementation of the PasswordResetController 
    //  class, which manages password reset requests and processes password 
    //  changes.
    ///
    /// Class Purpose:
    /// PasswordResetController class provides API endpoints for requesting
    //  password resets and resetting passwords using valid tokens. It uses 
    /// EmailService to send reset emails and TokenService to generate and 
    //  validate tokens.
    ///
    /// Dependencies:
    /// EmailService: Sends password reset emails.
    /// TokenService: Generates and validates password reset tokens.
    /// PlotContext: Database context for accessing user data.
    /// RequestPasswordReset: Model for password reset requests.
    /// ResetPassword: Model for resetting passwords.
    ///
    /// Written by: Michael Polhill
    /// </summary>
    [Route("api/password-reset")]
    [ApiController]
    public class PasswordResetController : ControllerBase
    {
        // CONSTANTS -- CONSTANTS -- CONSTANTS -- CONSTANTS -- CONSTANTS ------

        // Base link for the password reset page.
        // (Need to change when we have the actual route)
        private const string RESET_LINK_TEMPLATE 
            = "https://www.plot.com/reset-password?token=";
        
        // VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES ------

        // Service used for sending password reset emails.
        private readonly EmailService _emailService;

        // Database context for accessing user data.
        private readonly PlotContext _context;

        // Service used for generating and validating password reset tokens.
        private readonly TokenService _tokenService;

        // Methods -- Methods -- Methods -- Methods -- Methods -- Methods -----

        /// <summary>
        /// Constructor for PasswordResetController. Initializes the 
        /// services and context via dependency injection.
        /// </summary>
        /// <param name="emailService"></param>
        /// <param name="context"></param>
        /// <param name="tokenService"></param>
        public PasswordResetController(EmailService emailService, 
            PlotContext context, TokenService tokenService)  
        {
            _emailService = emailService;
            _context = context;
            _tokenService = tokenService;
        }
        

        /// <summary>
        /// Endpoint to request a password reset. Takes a users email address 
        /// and verifies if the user exists. If the user exists, a new token is
        /// generated, a reset link is created, and an email with the reset 
        /// link is sent to the user.  Always returns a 200 OK response,
        /// to prevent email enumeration attacks.
        /// </summary>
        /// <param name="receivedEmailRequest">Model with a users email.
        //  </param>
        /// <returns>200 OK response regardless of email validity.</returns>
        [HttpPost("request-password-reset")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RequestPasswordReset(
            [FromBody] RequestPasswordReset receivedEmailRequest)
        {
            //Set the received email address to a local variable.
            var receivedEmail = receivedEmailRequest.EmailAddress;
            
            // If the email is null, return OK.
            if (receivedEmail == null)
            {
                return Ok();
            }

            // Find the user in the database via the email address.
            // Set the local user object to the found user.
            var user = _context.Users.FirstOrDefault
                (u => u.Email == receivedEmail);

            // If the user is null/not in the database, return OK.
            if (user == null)
            {
                return Ok();
            }

            // Generate a new token for the user.
            string resetToken = _tokenService.GenerateToken(receivedEmail);

            // Add the token to the end of the reset link template to create
            // a unique reset link.
            string resetLink = RESET_LINK_TEMPLATE + resetToken;

            //Set the users name and email to local variables.
            string name = user.FirstName ?? string.Empty;
            string email = user.Email ?? string.Empty;

            // Pass the users name, email, and reset link to the email service
            // to send the password reset email.
            await _emailService.SendPasswordResetEmailAsync(
                email, name, resetLink);
        
            // Return OK.
            return Ok();
        }


        /// <summary>
        /// Endpoint to reset a user's password. Takes a new password and a 
        /// token, validates the token, hashes the new password, and updates
        /// the user's password in the database.
        /// </summary>
        /// <param name="receivedResetPassword">Model contains new password 
        /// and the token in the url</param>
        /// <returns>200 ok if Reset is successful, 400 bad request otherwise.
        //  </returns>
        [HttpPost("reset-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ResetPassword(
            [FromBody] ResetPassword receivedResetPassword)
        {
            // Set local variables from the received model.
            var token = receivedResetPassword.Token;
            var newPassword = receivedResetPassword.NewPassword;

            // If the token or new password is null, return bad request.
            if (token == null || newPassword == null)
            {
                return BadRequest("Token or password hash not found.");
            }


            // Validate the token, sends back the email address if valid.
            // Otherwise returns null.
            var email = _tokenService.ValidateToken(token);


            // If the email is null, return bad request.
            if (email == null)
            {
                
                return BadRequest("Invalid token.");
            }

            // Find the user in the database via the email address.
            // Set the local user object to the found user.
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            // If the user is null, return bad request.
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            // At this point the user is valid, create
            // a new password hasher.
            PasswordHasher<User> hasher = new PasswordHasher<User>();
            
            // Hash the new password.
            var hashedPassword = hasher.HashPassword(user, newPassword);

            // Set the users password to the hashed password.
            user.Password = hashedPassword;

            // Save the changes to the database.
            _context.SaveChanges();

            // Return OK with success message.
            return Ok("Password reset successful.");
        }
    }
}