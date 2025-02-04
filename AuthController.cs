/*
    Filename: AuthController.cs
    Part of Project: PLOT/PLOT-BE
    File Purpose:
    This file contains the authentication endpoints for users.
    Written by: Jordan Houlihan
*/

using System.Security.Claims;
using Dapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Plot;

[ApiController]
[Route("")]
public class AuthController : ControllerBase
{
    [HttpPost]
    [Route("/register")]
    public async Task<IResult> Register(User user)
    {
        try
        {
            using SqlConnection connection = new(Environment.GetEnvironmentVariable("DB_CONNECTION"));
            var checkIfEmailExistsSQL = "SELECT EMAIL " +
                                        "FROM Users " +
                                        "WHERE EMAIL = @EMAIL";
            object checkIfEmailExistsParameters = new { EMAIL = user.Email };
            var EmailExistsProduct = await connection.QuerySingleOrDefaultAsync(checkIfEmailExistsSQL, checkIfEmailExistsParameters);
            if (EmailExistsProduct != null || user.Password == null)
            {
                return Results.BadRequest();
            }

            PasswordHasher<User> passwordHasher = new();
            user.Password = passwordHasher.HashPassword(user, user.Password!);
            var insertUserSQL = "INSERT INTO Users " +
                                "(FIRST_NAME, LAST_NAME, EMAIL, PASSWORD, ROLE_TUID) " +
                                "VALUES " +
                                "(@FIRST_NAME, @LAST_NAME, @Email, @Password, @ROLE_TUID)";
            object[] insertUserParameters =
            [new
            {
                FIRST_NAME = user.FirstName,
                LAST_NAME = user.LastName,
                EMAIL = user.Email,
                PASSWORD = user.Password,
                ROLE_TUID = user.Role
            }];
            int rowsAffected = await connection.ExecuteAsync(insertUserSQL, insertUserParameters);
            if (rowsAffected == 0)
            {
                return Results.InternalServerError();
            }
            var enteredUserSQL = "SELECT TUID, FIRST_NAME, LAST_NAME, EMAIL, ROLE_TUID " +
                                 "FROM Users " +
                                 "WHERE EMAIL = @EMAIL";
            object enteredUserParameters = new { EMAIL = user.Email };
            var enteredUserProduct = await connection.QuerySingleAsync(enteredUserSQL, enteredUserParameters);

            var claims = new List<Claim>
            {
                new(ClaimTypes.Email, enteredUserProduct.EMAIL)
            };
            var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
            await HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties()
            {
                IsPersistent = true,
            });
            return Results.Ok(enteredUserProduct);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
        return Results.InternalServerError();
    }

    [HttpPost]
    [Route("/login")]
    public async Task<IResult> Login(UserLoginDTO user)
    {
        try
        {
            using SqlConnection connection = new(Environment.GetEnvironmentVariable("DB_CONNECTION"));
            var checkIfEmailExistsSQL = "SELECT EMAIL " +
                                        "FROM Users " +
                                        "WHERE EMAIL = @EMAIL";
            object checkIfEmailExistsParameters = new { EMAIL = user.Email };
            var emailExistsProduct = await connection.QuerySingleOrDefaultAsync(checkIfEmailExistsSQL, checkIfEmailExistsParameters);
            if (emailExistsProduct == null || user.Password == null)
            {
                return Results.BadRequest();
            }

            var getUserPasswordSQL = "SELECT PASSWORD " +
                                     "FROM Users " +
                                     "WHERE EMAIL = @EMAIL";
            object getUserPasswordParameters = new { EMAIL = user.Email };
            var getUserPasswordProduct = await connection.QuerySingleOrDefaultAsync(getUserPasswordSQL, getUserPasswordParameters);
            if (getUserPasswordProduct == null)
            {
                return Results.BadRequest(getUserPasswordProduct);
            }

            PasswordHasher<UserLoginDTO> passwordHasher = new();
            if (passwordHasher.VerifyHashedPassword(user, getUserPasswordProduct!.PASSWORD, user.Password) == PasswordVerificationResult.Failed)
            {
                return Results.BadRequest();
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Email, emailExistsProduct!.EMAIL)
            };
            var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
            await HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties()
            {
                IsPersistent = true,
            });
            return Results.Ok();
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
        return Results.InternalServerError();
    }

    [Authorize(Policy = "User-Policy")]
    [HttpPost]
    [Route("/logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("CookieAuth", new AuthenticationProperties()
        {
            IsPersistent = true,
        });
        return Ok();
    }
}