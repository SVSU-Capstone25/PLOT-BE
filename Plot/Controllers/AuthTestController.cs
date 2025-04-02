/*
    Filename: AuthController.cs
    Part of Project: PLOT/PLOT-BE/Plot/Controllers
    File Purpose:
    This file contains the authentication endpoint mappings
    for PLOT.

    Dependencies:
    EmailService: Sends password reset emails.
    TokenService: Generates and validates password reset tokens.
    AuthContext: Database context for accessing user data.

    Written by: Michael Polhill, Jordan Houlihan
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
using Microsoft.AspNetCore.Http;

namespace Plot.Plot.Tests;


[ApiController]
[Route("api/[controller]")]
public class AuthTestController : ControllerBase
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
    public AuthTestController(EmailService emailService, TokenService tokenService, IAuthContext authContext, EnvironmentSettings envSettings)
    {
        _emailService = emailService;
        _tokenService = tokenService;
        _authContext = authContext;
        _envSettings = envSettings;

        RESET_LINK_TEMPLATE = $"{_envSettings.audience}/reset-password?token=";
    }


    // Small test for endpoints----------------------------------------------------------------------------


    [HttpPost("test-get-token")]
    public IActionResult testGetToken()
    {
        var user = new User
        {
            Email="email@email.com",
            Role=3,
            UserId=1
        };

        var testToken=_tokenService.GenerateToken(user);

        return Ok(testToken);
    }

    [HttpGet("test-validate")]
    public IActionResult testValidate()
    {
        Console.WriteLine(Request.Headers);

        var authHeader = Request.Headers["Authorization"].ToString();
        Console.WriteLine(authHeader);
        var token = authHeader.Substring("Bearer ".Length).Trim();
        Console.WriteLine(token);


        return Ok();
    }


    [Authorize(Policy = "Owner")]
    [HttpPost("auth-test")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult TestPass()
    {
        Console.WriteLine("Pass");
        return Ok();
    }

    [Authorize]
    [HttpPost("data-test")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<ResetPasswordRequest> TestFail([FromBody] ResetPasswordRequest email)
    {
        email.EmailAddress = "new@email.com";
        return Ok(email);
    }

    [HttpPost("test-password")]
    public async Task<ActionResult<string>> TestPassword()
    {
        PasswordHasher<User> hasher = new();
        User user = new() { FirstName = "admin", LastName = "admin", Email = "NickLeja@email.com", Password = "admin", Role = 1, Active = true };

        LoginRequest newUserInfo = new()
        {
            Email = user.Email,
            Password = hasher.HashPassword(user, "admin")
        };

        int rowsAffected = await _authContext.UpdatePassword(newUserInfo);

        return Ok(rowsAffected);
    }
}