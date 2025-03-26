/*
    Filename: ClaimParserService.cs
    Part of Project: PLOT/PLOT-BE/Plot/Services

    File Purpose: This file defines the ClaimParserService class,
    which will be used to parse claims from the user's JWT token
    so they can be further authorized based on their roles/permissions.

    Written by: Jordan Houlihan
*/

using System.Security.Claims;

namespace Plot.Services;

public class ClaimParserService
{
    private const string USER_ID_CLAIM = "UserId";
    private const string USER_ROLE_CLAIM = "Role";
    private const string USER_EMAIL_CLAIM = "Email";

    public int? GetUserId(ClaimsPrincipal User)
    {
        var loggedInUserIdClaim = User.FindFirst(USER_ID_CLAIM);

        if (loggedInUserIdClaim == null)
        {
            return null;
        }


        if (!int.TryParse(loggedInUserIdClaim.Value, out int loggedInUserId))
        {
            return null;
        }

        return loggedInUserId;
    }

    public int? GetRoleId(ClaimsPrincipal User)
    {
        var loggedInRoleIdClaim = User.FindFirst(USER_ROLE_CLAIM);

        if (loggedInRoleIdClaim == null)
        {
            return null;
        }


        if (!int.TryParse(loggedInRoleIdClaim.Value, out int loggedInRoleId))
        {
            return null;
        }

        return loggedInRoleId;
    }

    public string? GetEmail(ClaimsPrincipal User)
    {
        var loggedInEmailClaim = User.FindFirst(USER_EMAIL_CLAIM);

        if (loggedInEmailClaim == null)
        {
            return null;
        }

        return loggedInEmailClaim.Value;
    }
}