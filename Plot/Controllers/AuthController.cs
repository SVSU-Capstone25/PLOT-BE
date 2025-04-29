/*
    Filename: AuthController.cs
    Part of Project: PLOT/PLOT-BE/Plot/Controllers
    File Purpose:
    This file contains the authentication endpoint mappings
    for PLOT.

    Written by: Michael Polhill, Jordan Houlihan
    Refactored comments Michael P. 4/28
*/

using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Plot.Data.Models.Auth.Login;
using Plot.Data.Models.Auth.Registration;
using Plot.Data.Models.Auth.ResetPassword;
using Plot.Data.Models.Env;
using Plot.Data.Models.Users;
using Microsoft.AspNetCore.Authentication;
using Plot.DataAccess.Interfaces;
using Plot.Services;
using System.Security.Claims;
using System.Net.Mime;
using System.Text.Json;
using Plot.Data.Models.Error;

namespace Plot.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    // VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES ------


    // Holds the link for a password reset
    private readonly string password_reset_link;

    // Service used for sending password reset emails.
    private readonly EmailService _emailService;

    // Service used for generating and validating jwt tokens.
    private readonly TokenService _tokenService;

    // Context that holds auth related methods to communicate with the DB.
    private readonly IAuthContext _authContext;
    
    // Service to access variable stored in the env file
    private readonly EnvironmentSettings _envSettings;


    // Methods -- Methods -- Methods -- Methods -- Methods -- Methods -----


    /// <summary>
    /// Constructor for AuthController. Initializes the 
    /// services and context via dependency injection.
    /// </summary>
    /// <param name="emailService"></param>
    /// <param name="tokenService"></param>
    /// <param name="authContext"></param>
    /// <param name="envSettings"></param>
    public AuthController(EmailService emailService, TokenService tokenService,
                         IAuthContext authContext, EnvironmentSettings envSettings)
    {
        _emailService = emailService;
        _tokenService = tokenService;
        _authContext = authContext;
        _envSettings = envSettings;
        //Get the audience from the env file to use in the reset link.
        password_reset_link = $"{_envSettings.audience}/password-reset?token=";
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
        var user = await _authContext.GetUserByEmail(receivedEmailRequest.EMAIL);

        if (user == null)
        {
            return Ok();
        }

        // Generate a new token for the user.
        string resetToken = _tokenService.GeneratePasswordResetToken(user);

        // Add the token to the end of the reset link template to create
        // a unique reset link.
        string resetLink = password_reset_link + resetToken;

        // Pass the users name, email, and reset link to the email service
        // to send the password reset email.
        await _emailService.SendPasswordResetEmailAsync(
            user.EMAIL!, user.FIRST_NAME!, resetLink);

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
        var email = _tokenService.ValidatePasswordResetToken(token);


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
            EMAIL = email,
            PASSWORD = hasher.HashPassword(user, receivedResetPassword.NewPassword)
        };


        // Get the number of rows affected from db to determine if
        // operation succeeded.
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
    /// Note: On user creation a random hashed password is initially set as
    /// the users password, this is why the user receives a password reset
    /// email on registration.
    /// </summary>
    /// <param name="user">The new user</param>
    /// <returns>200 ok if Reset is successful, 
    /// 400 bad request otherwise.</returns>
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

        var registeredUser = await _authContext.GetUserByEmail(user.EMAIL!);

        if (registeredUser == null)
        {
            return BadRequest();
        }

        // Generate token for password reset.
        string resetToken = _tokenService.GeneratePasswordResetToken(registeredUser!);
        string resetLink = password_reset_link + resetToken;

        //Send email so that the new user can set their password.
        await _emailService.SendRegistrationEmailAsync(registeredUser.EMAIL!, registeredUser.FIRST_NAME!, resetLink);

        return Ok();
    }

    /// <summary>
    /// This endpoint deals with logging in a user. This will verify if
    /// the email and password combination is correct, then send a jwt token
    /// to the frontend to add to set a cookie for authorization.
    /// </summary>
    /// <param name="userLoginAttempt">The login attempt</param>
    /// <returns>The token model used for authorization or bad request</returns>
    [HttpPost("login")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Login(LoginRequest userLoginAttempt)
    {
        // Determine if incoming json can bind to the model
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        

        var user = await _authContext.GetUserByEmail(userLoginAttempt.EMAIL!);

        
        if (user == null || userLoginAttempt.PASSWORD == null)
        {
            ErrorMessage errorMessage = new ErrorMessage() { Message = "Invalid login."};
            return BadRequest(errorMessage);
        }

        PasswordHasher<User> passwordHasher = new();

        if (passwordHasher.VerifyHashedPassword(user, user.PASSWORD!, userLoginAttempt.PASSWORD) == PasswordVerificationResult.Failed)
        {
            ErrorMessage errorMessage = new ErrorMessage() { Message = "Invalid login."};
            return BadRequest(errorMessage);
        }

        // Generate jwt token for authorization
        var token = _tokenService.GenerateAuthToken(user);

        return Ok(new LoginToken()
        {
            Token = token
        });
    }


    /// <summary>
    /// This endpoint deals with logging out a user. This will delete the token 
    /// from the user.
    /// NOTE: NOT CURRENTLY BEING USED.
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult Logout()
    {
        //Reset the token in the response cookies for authentication.
        // This will delete the token from the user.
        var token = "";
        Response.Cookies.Append("Auth", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTimeOffset.UtcNow.AddMinutes(-5),//Browser will delete the cookie due to old experation
        });

        return Ok();
    }

    /// <summary>
    /// This method determines the current user from the 
    /// jwt in the http Authorization header and sends back
    /// a userDTO if user is found. 
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet("get-current-user")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserDTO?>> GetCurrentUser()
    {
        string? authHeader = HttpContext.Request.Headers["Authorization"];


        if (string.IsNullOrEmpty(authHeader))
        {
            return BadRequest();
        }
        
        //Trim the header value to get the token
        string token = authHeader.Substring("Bearer ".Length).Trim();

        //Validate and get the users email from the token
        var userEmail = _tokenService.ValidateAuthToken(token);

        var user = await _authContext.GetUserByEmail(userEmail!);

        UserDTO userDTO = new UserDTO()
        {
            TUID=user?.TUID,
            FIRST_NAME=user?.FIRST_NAME,
            LAST_NAME=user?.LAST_NAME,
            EMAIL=user?.EMAIL,
            ROLE=user?.ROLE
        };

        return Ok(userDTO);
    }
}