/*
    Filename: ResetPasswordRequest.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Auth/ResetPassword

    File Purpose:
    This file defines the ResetPasswordRequest model class, 
    an EmailAddress for a post http request.

    Class Purpose:
    This record serves as a model for data to inject into a post http
    request in the AuthController.
    The email address is used to send a password reset email.

    Written by: Michael Polhill
*/

namespace Plot.Data.Models.Auth.ResetPassword;

public record ResetPasswordRequest
{
    // VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES ------

    // Email address of the user requesting a password reset.
    public required string EMAIL { get; set; }
}