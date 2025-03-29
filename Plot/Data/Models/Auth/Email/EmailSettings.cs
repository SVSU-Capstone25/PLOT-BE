/*
    Filename: EmailSettings.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Auth/Email

    File Purpose:
    This file defines the EmailSettings model class, 
    which holds configuration for EmailService.

    Class Purpose:
    This record serves as a model for email settings, each property
    corresponding to a specific email configuration value needed by
    the EmailService class.

    Written by: Michael Polhill
*/

namespace Plot.Data.Models.Auth.Email;

public record EmailSettings
{
    // VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES ------

    // SMPT server that the senders email is hosted on.
    public string? SmtpServer { get; set; }
    // Port that the senders SMTP server is using.
    public int? SmtpPort { get; set; }
    // Senders Password for the SMTP server(Email Password).
    public string? SenderSmtpPass { get; set; }
    // Senders Email Address.
    public string? SenderEmail { get; set; }
    // Senders Name.
    public string? SenderName { get; set; }
}