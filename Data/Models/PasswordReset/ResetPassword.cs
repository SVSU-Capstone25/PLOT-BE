namespace Plot.Data.Models.PasswordReset
{
    /// <summary>
    /// Filename: ResetPassword.cs
    /// Part of Project: PLOT (can rename later)
    /// 
    /// File Purpose:
    /// This file defines the ResetPassword model class, 
    /// a NewPassword and Token for a post http request.
    /// 
    /// Class Purpose:
    /// This class serves as a model for data to inject into a post http
    /// request in the PasswordResetController API.
    /// The new password and token are used to reset a users password.
    /// 
    /// Written by: Michael Polhill
    /// </summary>
    public class ResetPassword
    {
        // VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES ------

        // New password for the user.
        public required string NewPassword { get; set; }

        // Token used when the link was generated to validate the password reset.
        public required string Token { get; set; }
    }
}
