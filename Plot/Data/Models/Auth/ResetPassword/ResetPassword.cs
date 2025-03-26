/*
    Filename: ResetPassword.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Auth/ResetPassword
    
    File Purpose:
    This file defines the ResetPassword model class, 
    a NewPassword and Token for a post http request.

    Class Purpose:
    This record serves as a model to inject into a post http
    request in the AuthController.
    The new password and token are used to reset a users password.

    Written by: Michael Polhill
*/

namespace Plot.Data.Models.Auth.ResetPassword;

public record ResetPassword
{
    // VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES ------

    // New password for the user.
    public required string NewPassword { get; set; }

    // Token used when the link was generated to validate the password reset.
    public required string Token { get; set; }
}