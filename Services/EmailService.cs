using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Plot.Data.Models.Email;
using Plot.Utility;


//80 chars---------------------------------------------------------------------
namespace Plot.Services 
{
    /// <summary>
    /// Filename: EmailService.cs
    /// Part of Project: PLOT (can rename later)
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
        private const string PLATO_LOGO_FILE_PATH = "Images/PlatoLogo.png";
        
        // VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES ------

        // Name of the email sender.
        private readonly string _senderName= string.Empty;  

        // Email address of the sender.
        private readonly string _senderEmail= string.Empty; 

        // SMTP server address. ex:"smtp.gmail.com"
        private readonly string _smtpServer= string.Empty;  

        // SMTP server port number
        private readonly int _smtpPort= 0; 

        // SMTP server password 
        private readonly string _senderSmtpPass= string.Empty;  

        // Methods -- Methods -- Methods -- Methods -- Methods -- Methods -----
        /// <summary>
        /// Constructor used for EmailService class.
        /// Gets necessary email settings from an IOptions instance.
        /// </summary>
        /// <param name="emailSettings">An instance of IOptions that contains
        /// application settings.</param> 
        /// <exception cref="ArgumentNullException"> Thrown when an email 
        /// configuration is missing. </exception>
        public EmailService(IOptions<EmailSettings> emailSettings) 
        {
            //Use the emailSettings instance to get the email settings.
            _senderName=emailSettings.Value.SenderName ?? throw new 
                ArgumentNullException(emailSettings.Value.SenderName);

            _senderEmail=emailSettings.Value.SenderEmail ?? throw new 
                ArgumentNullException(emailSettings.Value.SenderEmail);

            _smtpServer=emailSettings.Value.SmtpServer ?? throw new 
                ArgumentNullException(emailSettings.Value.SmtpServer);

            _smtpPort=emailSettings.Value.SmtpPort ?? throw new 
                ArgumentNullException(nameof(emailSettings.Value.SmtpPort));

            _senderSmtpPass=emailSettings.Value.SenderSmtpPass ?? throw new 
                ArgumentNullException(emailSettings.Value.SenderSmtpPass);
        }


        /// <summary>
        /// This method sends a password reset email to the specified recipient.
        /// It sends a pre-defined email template to an instance of 
        /// RazorEmailRenderer to render the html email body. The email is then
        /// sent using the SendEmailAsync method.
        /// </summary>
        /// <param name="recipientName"> The name of the email recipient.</param>
        /// <param name="recipientEmailAddress">The email address of the 
        /// recipient.</param> 
        /// <param name="resetLink"></param> The password reset link.
        /// <returns></returns>
        public async Task SendPasswordResetEmailAsync(
            string recipientName, string recipientEmailAddress, string resetLink)
        {
            //Instantiate a new RazorEmailRenderer instance, it will be used to 
            // render the html template.
            var razorEmailRenderer = new RazorEmailRenderer();
            //Create a new EmailTemplate instance with the email body content.
            var emailTemplate = new EmailTemplate
            {
                Name = recipientName,
                BodyText = "We received a request to reset your password." + 
                    "  Please click the button below to reset it.",
                ButtonText = "Reset Password",
                ButtonLink = resetLink,
                AfterButtonText = "If you did not request this," +
                    " you can safely ignore this email."
            };


            //Render the email template to send as the email body.
            string emailHtmlBody = await razorEmailRenderer.RenderEmailAsync(
                emailTemplate);
            //Set the email subject.
            string emailSubject = "Reset Your Password";

            //Instantiate a new MimeMessage instance with the email content.
            MimeMessage EmailMessage = BuildEmailMessage(recipientName, 
                recipientEmailAddress, emailSubject, emailHtmlBody);

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
        private MimeMessage BuildEmailMessage(string recipientName,
            string recipientEmailAddress, string subject, string htmlBody)
        {
            // Create a new email message.
            var message = new MimeMessage ();

            // Add senders and recipients name and email to the message.
            message.From.Add (new MailboxAddress (_senderName, _senderEmail));
            message.To.Add (new MailboxAddress (
                recipientName, recipientEmailAddress));

            // Add the email subject to the message.
            message.Subject = subject;

            // Initialize a new BodyBuilder instance to build the email body.
            BodyBuilder bodyBuilder = new BodyBuilder();

            // Get the path to the PLATO logo image file.
            string imagePath = Path.Combine(
                Directory.GetCurrentDirectory(), PLATO_LOGO_FILE_PATH);

            // Embed the PLATO logo image to be used as a header image.
            var linkedImage = bodyBuilder.LinkedResources.Add(imagePath);
            linkedImage.ContentId = "headerImage";

            // Replace the placeholder in the email body with the header image.
            bodyBuilder.HtmlBody = htmlBody.Replace(
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
}