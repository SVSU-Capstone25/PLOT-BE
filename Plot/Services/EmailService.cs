using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Plot.Data.Models.Auth.Email;
using Plot.Data.Models.Env;

namespace Plot.Services;

//80 chars---------------------------------------------------------------------
/// <summary>
/// Filename: EmailService.cs
/// Part of Project: PLOT/PLOT-BE/Plot/Services
/// 
/// File Purpose:
/// This file contains the implementation of the EmailService class, which
/// is responsible for sending emails for the application.
///
/// Class Purpose:
/// EmailService class holds all logic for sending emails. It is
/// designed to be used with dependency injection by accepting an IOptions
/// instance to retrieve the necessary settings. The class
/// retrieves settings from an IOptions instance. 
/// The email body uses a pre-rendered html template provided by 
/// RazorEmailRenderer. The class provides methods to compose and send
/// emails asynchronously using the
/// MailKit and MimeKit libraries.
/// 
/// Dependencies:
/// Mailkit/Mimekit: https://github.com/jstedfast/MailKit.
/// RazorEmailRenderer: A custom class that renders email templates using
//  Razorlight.
/// IConfiguration: An interface that provides access to configuration
//  settings.
/// EmailSettings: A model that holds email settings.
/// 
/// Written by: Michael Polhill
/// </summary>
public class EmailService
{
    // CONSTANTS -- CONSTANTS -- CONSTANTS -- CONSTANTS -- CONSTANTS ------
    // Path to the PLATO logo image file.
    private const string PLATO_LOGO_FILE_PATH = "Data/Images/PlatoLogo.png";

    // VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES ------

    // Name of the email sender.
    private readonly string _senderName;

    // Email address of the sender.
    private readonly string _senderEmail;

    // SMTP server address. ex:"smtp.gmail.com"
    private readonly string _smtpServer;

    // SMTP server port number
    private readonly int _smtpPort;

    // SMTP server password 
    private readonly string _senderSmtpPass;
    private readonly EnvironmentSettings _envSettings;

    // Methods -- Methods -- Methods -- Methods -- Methods -- Methods -----
    /// <summary>
    /// Constructor used for EmailService class.
    /// Gets necessary email settings from an IOptions instance.
    /// Gets the servers email password from .env file.
    /// </summary>
    /// <param name="emailSettings">An instance of IOptions that contains
    /// application settings.</param> 
    /// <exception cref="ArgumentNullException"> Thrown when an email 
    /// configuration is missing. </exception>
    public EmailService(IOptions<EmailSettings> emailSettings, EnvironmentSettings envSettings)
    {
        _envSettings = envSettings;
        //Use the emailSettings instance to get the email settings.
        _senderName = emailSettings.Value.SenderName ?? throw new
            ArgumentNullException(emailSettings.Value.SenderName);

        _senderEmail = emailSettings.Value.SenderEmail ?? throw new
            ArgumentNullException(emailSettings.Value.SenderEmail);

        _smtpServer = emailSettings.Value.SmtpServer ?? throw new
            ArgumentNullException(emailSettings.Value.SmtpServer);

        _smtpPort = emailSettings.Value.SmtpPort ?? throw new
            ArgumentNullException(nameof(emailSettings.Value.SmtpPort));

        _senderSmtpPass = _envSettings.email_pass;
    }


    /// <summary>
    /// This method sends a password reset email to the specified recipient.
    /// It sends a pre-defined email template with email sending info
    /// to BuildEmailMessage, and then the email is then
    /// sent using the SendEmailAsync method.
    /// </summary>
    /// <param name="recipientEmailAddress">The email address of the 
    /// recipient.</param> 
    /// <param name="recipientName"> The name of the email recipient.</param>
    /// <param name="resetLink"></param> The password reset link.
    /// <returns></returns>
    public async Task SendPasswordResetEmailAsync(
        string recipientEmailAddress, string recipientName, string resetLink)
    {
        // Create a new email template to be used as the 
        // password reset body.
        var emailTemplateBody = new EmailTemplate
        {
            Name = recipientName,
            BodyText = "We received a request to reset your password." +
                "  Please click the button below to reset it.",
            ButtonText = "Reset Password",
            ButtonLink = resetLink,
            AfterButtonText = "If you did not request this," +
                " you can safely ignore this email."
        };

        //Set the emails subject.
        var subject = "PLOT Password Reset";

        //Instantiate a new MimeMessage instance with the email content.
        MimeMessage EmailMessage = await BuildEmailMessage(recipientName,
            recipientEmailAddress, subject, emailTemplateBody);

        //Call the SendEmailAsync method to send the email.
        await SendEmailAsync(EmailMessage);
    }

    /// <summary>
    /// Method sends a successful registration email to the specified recipient.
    /// It sends a pre-defined email template with email sending info
    /// to BuildEmailMessage, and then the email is then
    /// sent using the SendEmailAsync method.
    /// </summary>
    /// <param name="recipientEmailAddress">The email address of the 
    /// recipient.</param> 
    /// <param name="recipientName"> The name of the email recipient.</param>
    /// <param name="loginLink"></param> The login reset link.
    /// <returns></returns>
    public async Task SendRegistrationEmailAsync(
        string recipientEmailAddress, string recipientName, string loginLink)
    {
        // Create a new email template to be used as the 
        // registration notification body.
        var emailTemplateBody = new EmailTemplate
        {
            Name = recipientName,
            BodyText = "Your account has been successfully registered" +
                "  with PLOT. Please click the button below to create a password.",
            ButtonText = "Change Password",
            ButtonLink = loginLink,
            AfterButtonText = string.Empty
        };

        //Set the emails subject.
        var subject = "PLOT Successful Registration";

        //Instantiate a new MimeMessage instance with the email content.
        MimeMessage EmailMessage = await BuildEmailMessage(recipientName,
            recipientEmailAddress, subject, emailTemplateBody);

        //Call the SendEmailAsync method to send the email.
        await SendEmailAsync(EmailMessage);
    }


    /// <summary>
    /// This method builds a new MimeMessage instance with the email 
    /// content. Email body is a rendered html template provided by 
    /// RazorEmailRenderer. This method also embeds the PLATO logo image to
    /// be used as a header image.
    /// </summary>
    /// <param name="recipientName">The name of the email recipient.</param>
    /// <param name="recipientEmailAddress">The email address of the 
    //  recipient.</param>
    /// <param name="subject">The subject of the email.</param>
    /// <param name="body"> html string for email content</param>
    /// <returns>A MimeMessage object ready to be sent as an email</returns>
    private async Task<MimeMessage> BuildEmailMessage(string recipientName,
        string recipientEmailAddress, string subject, EmailTemplate emailTemplateBody)
    {
        // Create a new email message.
        var message = new MimeMessage();

        // Add senders and recipients name and email to the message.
        message.From.Add(new MailboxAddress(_senderName, _senderEmail));
        message.To.Add(new MailboxAddress(
            recipientName, recipientEmailAddress));

        // Add the email subject to the message.
        message.Subject = subject;


        // Instantiate a new RazorEmailRenderer instance, it will be used to 
        // render the html template
        var razorEmailRenderer = new RazorEmailRenderer();

        //Render the email template to send as the email body.
        string emailHtmlBody = await razorEmailRenderer.RenderEmailAsync(
            emailTemplateBody);

        // Initialize a new BodyBuilder instance to build the email body.
        BodyBuilder bodyBuilder = new BodyBuilder();

        // Get the path to the PLATO logo image file.
        string imagePath = Path.Combine(
            Directory.GetCurrentDirectory(), PLATO_LOGO_FILE_PATH);

        // Embed the PLATO logo image to be used as a header image.
        var linkedImage = bodyBuilder.LinkedResources.Add(imagePath);
        linkedImage.ContentId = "headerImage";

        // Replace the placeholder in the email body with the header image.
        bodyBuilder.HtmlBody = emailHtmlBody.Replace(
            "{{HeaderImage}}", "cid:headerImage");

        // Set the email body to the message.
        message.Body = bodyBuilder.ToMessageBody();

        // Return the message.
        return message;
    }


    /// <summary>
    /// This method sends a MimeMessage instance as an email asynchronously
    /// using SMTP.
    /// </summary>
    /// <param name="message"> MimeMessage to be used as email contents
    /// </param>
    /// <returns>A task that represents the asynchronous email sending 
    //  operation. </returns>
    private async Task SendEmailAsync(MimeMessage message)
    {
        // Create an SmtpClient instance to send the message.
        using var client = new SmtpClient();
        try
        {
            //Connect to the SMTP server
            await client.ConnectAsync(_smtpServer,
                _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);

            // Authenticate the senders email credentials
            client.Authenticate(_senderEmail, _senderSmtpPass);

            // Send the email and disconnect from the server
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
        catch (Exception e)
        {
            // Print exceptions to the console.
            Console.WriteLine(e);
            Console.WriteLine("Email failed to send.");
        }
    }
}