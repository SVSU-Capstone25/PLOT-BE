using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Plot.Data.Models.Email;

namespace Plot.Services 
{
    /// <summary>
    /// Filename:  EmailSender.cs
    ///Part of Project: PLOT (can rename later)
    /// 
    ///File Purpose:
    ///This file contains the implementation of the EmailSender class, which 
    ///is responsible for sending emails for the application.
    ///
    /// Class Purpose:
    /// EmailSender class holds all logic for sending emails. It is
    /// designed to be used with dependency injection by accepting an IConfiguration
    /// instance to retrieve the necessary configuration settings. The class
    /// retrieves configuration settings from an IConfiguration instance and
    /// provides methods to compose and send emails asynchronously using the
    /// MailKit and MimeKit libraries which can be found at 
    /// https://github.com/jstedfast/MailKit.
    /// </summary>
    public class EmailService
    {
        private const string PLATO_LOGO_FILE_PATH = "Images/PlatoLogo.png";
        
        // VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES
        private readonly string _senderName= string.Empty;  // Name of the email sender.
        private readonly string _senderEmail= string.Empty; // Email address of the sender.
        private readonly string _smtpServer= string.Empty;  // SMTP server address. ex:"smtp.gmail.com"
        private readonly int _smtpPort= 0; // SMTP server port number
        private readonly string _senderSmtpPass= string.Empty;  // SMTP server password 

        


        // METHODS/FUNCTIONS -- METHODS/FUNCTIONS -- METHODS/FUNCTIONS

        /// <summary>
        /// Constructor used for EmailSender class.
        /// Gets necessary email settings from an IConfiguration instance.
        /// </summary>
        /// <param name="config"></param> An instance of IConfiguration that contains application settings.
        /// <exception cref="ArgumentNullException"> Thrown when an email configuration is missing.
        /// </exception>
        public EmailService(IOptions<EmailSettings> emailSettings) 
        {
            _senderName=emailSettings.Value.SenderName
                ?? throw new ArgumentNullException(emailSettings.Value.SenderName);

            _senderEmail=emailSettings.Value.SenderEmail
                ?? throw new ArgumentNullException(emailSettings.Value.SenderEmail);

            _smtpServer=emailSettings.Value.SmtpServer
                ?? throw new ArgumentNullException(emailSettings.Value.SmtpServer);

            _smtpPort=emailSettings.Value.SmtpPort 
                ?? throw new ArgumentNullException(nameof(emailSettings.Value.SmtpPort));

            _senderSmtpPass=emailSettings.Value.SenderSmtpPass
                ?? throw new ArgumentNullException(emailSettings.Value.SenderSmtpPass);
        }

//BEGIN------------------TEMP METHODS------TEMP METHODS------TEMP METHODS------------------
//Just some placeholder methods for now
        public async Task SendPasswordResetEmailAsync(
            string recipientName, string recipientEmailAddress, string resetLink)
        {
            var razorEmailRenderer = new RazorEmailRenderer();
            var emailTemplate = new EmailTemplate
            {
                Name = recipientName,
                BodyText = "We received a request to reset your password. Please click the button below to reset it.",
                ButtonText = "Reset Password",
                ButtonLink = resetLink,
                AfterButtonText = "If you did not request this, you can safely ignore this email."
            };


            string emailHtmlBody = await razorEmailRenderer.RenderEmailAsync(emailTemplate);
            string emailSubject = "Reset Your Password";

            MimeMessage EmailMessage = BuildEmailMessage(
                recipientName, recipientEmailAddress, emailSubject, emailHtmlBody);

            await SendEmailAsync(EmailMessage);
        }



//Not exactly sure what notifications were sending
        
 //END------------------TEMP METHODS------TEMP METHODS------TEMP METHODS------------------       

        /// <summary>
        /// Asynchronously sends an email to the specified recipient.
        /// </summary>
        /// <param name="recipientName">The name of the email recipient.</param>
        /// <param name="recipientEmailAddress">The email address of the recipient.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="body">The body content of the email.</param>
        /// <returns>A task that represents the asynchronous email sending operation.</returns>
        private MimeMessage BuildEmailMessage(
            string recipientName, string recipientEmailAddress, string subject, string htmlBody)
        {
            // Create a new email message.
            var message = new MimeMessage ();

            // Add senders and recipients name and email to the message.
            message.From.Add (new MailboxAddress (_senderName, _senderEmail));
            message.To.Add (new MailboxAddress (recipientName, recipientEmailAddress));

            // Add the email subject to the message.
            message.Subject = subject;

            BodyBuilder bodyBuilder = new BodyBuilder();

            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), PLATO_LOGO_FILE_PATH);
            var linkedImage = bodyBuilder.LinkedResources.Add(imagePath);
            linkedImage.ContentId = "headerImage";

            bodyBuilder.HtmlBody = htmlBody.Replace("{{HeaderImage}}", "cid:headerImage");
            
            message.Body = bodyBuilder.ToMessageBody();
            return message;
            }



            private async Task SendEmailAsync(MimeMessage message)
            {

            // Create an SmtpClient instance to send the message.
            using var client = new SmtpClient();
            try
            {
                //Connect to the SMTP server
                await client.ConnectAsync(
                    _smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);

                // Authenticate the senders email credentials
                // Note: only needed if the SMTP server requires authentication.
                
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
