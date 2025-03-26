/*
    Filename: AuthController.cs
    Part of Project: PLOT/PLOT-BE/Controllers
    File Purpose:
    This file contains the authentication endpoint mappings
    for PLOT.

    Dependencies:
    EmailService: Sends password reset emails.
    TokenService: Generates and validates password reset tokens.
    AuthContext: Database context for accessing user data.

    Written by: Michael Polhill, Jordan Houlihan
*/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Plot.Data.Models.Auth.Login;
using Plot.Data.Models.Auth.Registration;
using Plot.Data.Models.Auth.ResetPassword;
using Plot.Data.Models.Env;
using Plot.Data.Models.Users;
using Plot.DataAccess.Interfaces;
using Plot.Services;

namespace Plot.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    // CONSTANTS -- CONSTANTS -- CONSTANTS -- CONSTANTS -- CONSTANTS ------

    // Base link for the password reset page.
    // (Need to change when we have the actual route)
    private readonly string RESET_LINK_TEMPLATE;

    // VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES ------

    // Service used for sending password reset emails.
    private readonly EmailService _emailService;

    // Service used for generating and validating password reset tokens.
    private readonly TokenService _tokenService;

    private readonly IAuthContext _authContext;
    private readonly EnvironmentSettings _envSettings;
    // Methods -- Methods -- Methods -- Methods -- Methods -- Methods -----

    /// <summary>
    /// Constructor for AuthController. Initializes the 
    /// services and context via dependency injection.
    /// </summary>
    /// <param name="emailService"></param>
    /// <param name="tokenService"></param>
    /// <param name="authContext"></param>
    public AuthController(EmailService emailService, TokenService tokenService, IAuthContext authContext, EnvironmentSettings envSettings)
    {
        _emailService = emailService;
        _tokenService = tokenService;
        _authContext = authContext;
        _envSettings = envSettings;

        RESET_LINK_TEMPLATE = $"{_envSettings.audience}/reset-password?token=";
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
    [HttpPost("reset-password-request")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> ResetPasswordRequest([FromBody] ResetPasswordRequest receivedEmailRequest)
    {
        var user = await _authContext.GetUserByEmail(receivedEmailRequest.EmailAddress);

        if (user == null)
        {
            return Ok();
        }

        // Generate a new token for the user.
        string resetToken = _tokenService.GenerateToken(user);

        // Add the token to the end of the reset link template to create
        // a unique reset link.
        string resetLink = RESET_LINK_TEMPLATE + resetToken;

        // Pass the users name, email, and reset link to the email service
        // to send the password reset email.
        await _emailService.SendPasswordResetEmailAsync(
            user.Email!, user.FirstName!, resetLink);
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
    public async Task<ActionResult> ResetPassword([FromBody] ResetPassword receivedResetPassword)
    {
        // Set local variables from the received model.
        var token = receivedResetPassword.Token;
        var newPassword = receivedResetPassword.NewPassword;

        // If the token or new password is null, return bad request.
        if (token == null || newPassword == null)
        {
            return BadRequest("Invalid request.");
        }

        // Validate the token, sends back the email address if valid.
        // Otherwise returns null.
        var email = _tokenService.ValidateToken(token);


        // If the email is null, return bad request.
        if (email == null)
        {
            return BadRequest("Invalid request.");
        }

        var user = await _authContext.GetUserByEmail(email);

        if (user == null)
        {
            return BadRequest("User not found.");
        }

        PasswordHasher<User> hasher = new();

        LoginRequest newUserInfo = new()
        {
            Email = email,
            Password = hasher.HashPassword(user, receivedResetPassword.NewPassword)
        };

        int rowsAffected = await _authContext.UpdatePassword(newUserInfo);

        if (rowsAffected > 0)
        {
            // Return OK with success message.
            return Ok("Password reset successful.");
        }
        return BadRequest();
    }

    /// <summary>
    /// This endpoint deals with registering a user. This will create the
    /// user sent from the frontend, find the user in the database to ensure
    /// that the process succeeded, and send the user a registration email
    /// to change their password.
    /// </summary>
    /// <param name="user">The new user</param>
    /// <returns></returns>
    [Authorize(Policy = "Manager")]
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Register(UserRegistration user)
    {
        int success = await _authContext.CreateUser(user);

        if (success == 0)
        {
            return BadRequest();
        }

        var registeredUser = await _authContext.GetUserByEmail(user.Email!);

        if (registeredUser == null)
        {
            return BadRequest();
        }

        string resetToken = _tokenService.GenerateToken(registeredUser!);
        string resetLink = RESET_LINK_TEMPLATE + resetToken;

        await _emailService.SendRegistrationEmailAsync(registeredUser.Email!, registeredUser.FirstName!, resetLink);

        return Ok();
    }

    /// <summary>
    /// This endpoint deals with logging in a user. This will verify if
    /// the email and password combination is correct, then send a token
    /// to the frontend to add to their local storage for further processing
    /// when the user enters the dashboard.
    /// </summary>
    /// <param name="userLoginAttempt">The login attempt</param>
    /// <returns>The token used for authorization</returns>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Login(LoginRequest userLoginAttempt)
    {
        var user = await _authContext.GetUserByEmail(userLoginAttempt.Email!);

        if (user == null || userLoginAttempt.Password == null)
        {
            return BadRequest();
        }

        PasswordHasher<User> passwordHasher = new();


        if (passwordHasher.VerifyHashedPassword(user, user.Password!, userLoginAttempt.Password) == PasswordVerificationResult.Failed)
        {
            return BadRequest();
        }

        var token = _tokenService.GenerateToken(user);

        Response.Cookies.Append("Auth", token, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddMinutes(30)
        });

        return Ok();
    }

    /// <summary>
    /// This endpoint deals with logging out a user. This will delete the token 
    /// from the user.
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult Logout()
    {
        return Ok();
    }
    // Small test for endpoints----------------------------------------------------------------------------
    [Authorize(Policy = "Owner")]
    [HttpPost("auth-test")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult TestPass()
    {
        Console.WriteLine("Pass");
        return Ok();
    }

    [HttpPost("data-test")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<ResetPasswordRequest> TestFail([FromBody] ResetPasswordRequest email)
    {
        email.EmailAddress = "new@email.com";
        return Ok(email);
    }
}