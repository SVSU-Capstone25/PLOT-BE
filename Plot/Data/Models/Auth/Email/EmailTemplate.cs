/*
    Filename: EmailTemplate.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Auth/Email
    
    File Purpose:
    This file defines the EmailTemplate model class, 
    it defines data to inject into an html email template.

    Class Purpose:
    This record serves as a model to inject into an html email
    template (emailTemplate.htmlcs), each property corresponds to 
    a specific data value needed by the email template.

    Written by: Michael Polhill
*/

namespace Plot.Data.Models.Auth.Email;

public record EmailTemplate
{
    // VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES ------
    // Name of the email recipient.
    public required string Name { get; set; }

    // Body text of the email.
    public required string BodyText { get; set; }

    // Button text for the email.
    public required string ButtonText { get; set; }

    // Link for the email button.
    public required string ButtonLink { get; set; }

    // Text to display after the button.
    public required string AfterButtonText { get; set; }
}