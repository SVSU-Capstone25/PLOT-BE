namespace Plot.Data.Models.PasswordReset

    /// <summary>
    /// Filename: RequestPasswordReset.cs
    /// 
    /// File Purpose:
    /// This file defines the RequestPasswordReset model class, 
    /// an EmailAddress for a post http request.
    /// 
    /// Class Purpose:
    /// This class serves as a model for data to inject into a post http
    /// request in the PasswordResetController API.
    /// The email address is used to send a password reset email.
    /// 
    /// Written by: Michael Polhill
    /// </summary>
{
    public class RequestPasswordReset
    {
        // VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES ------

        // Email address of the user requesting a password reset.
        public required string EmailAddress { get; set; }
    }
}
