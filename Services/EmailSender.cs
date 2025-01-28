using MailKit.Net.Smtp;
using MimeKit;

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
    class EmailSender
    {
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
        public EmailSender(IConfiguration config) 
        {
            // Use the IConfiguration instance to retrieve EmailSettings located in appsettings.json
            var emailSettings=config.GetSection("EmailSettings");

            // Assign email settings data to corresponding variable.
            _senderName=emailSettings["SenderName"]
                ?? throw new ArgumentNullException("Sender Name not configured in EmailSettings");

            _senderEmail=emailSettings["SenderEmail"]
                ?? throw new ArgumentNullException("Sender Email not configured in EmailSettings");

            _smtpServer=emailSettings["SmtpServer"]
                ?? throw new ArgumentNullException("Smtp Server not configured in EmailSettings");

            _smtpPort=int.Parse(emailSettings["SmtpPort"]
                ?? throw new ArgumentNullException("Smtp Port Number not configured in EmailSettings"));

            _senderSmtpPass=emailSettings["SenderSmtpPass"]
                ?? throw new ArgumentNullException("Sender Smtp password not configured in EmailSettings");
        }

//BEGIN------------------TEMP METHODS------TEMP METHODS------TEMP METHODS------------------
//Just some placeholder methods for now
        public async Task SendPasswordResetEmailAsync(
            string recipientName, string recipientEmailAddress, string resetLink)
        {
            string emailBody = "Please click this link to reset your password " + resetLink;
            string emailSubject = "Password reset link";

            await SendEmailAsync(
                recipientName, recipientEmailAddress, emailSubject, emailBody);
        }

//Not exactly sure what notifications were sending
        public async Task SendNotificationEmailAsync(
            string recipientName, string recipientEmailAddress, 
            string notificationSubject, string notificationBody)
        {
            await SendEmailAsync(
                recipientName, recipientEmailAddress, notificationSubject, notificationBody);
        }
 //END------------------TEMP METHODS------TEMP METHODS------TEMP METHODS------------------       

        /// <summary>
        /// Asynchronously sends an email to the specified recipient.
        /// </summary>
        /// <param name="recipientName">The name of the email recipient.</param>
        /// <param name="recipientEmailAddress">The email address of the recipient.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="body">The body content of the email.</param>
        /// <returns>A task that represents the asynchronous email sending operation.</returns>
        public async Task SendEmailAsync(
            string recipientName, string recipientEmailAddress, string subject, string body)
        {
            // Create a new email message.
            var message = new MimeMessage ();

            // Add senders and recipients name and email to the message.
            message.From.Add (new MailboxAddress (_senderName, _senderEmail));
            message.To.Add (new MailboxAddress (recipientName, recipientEmailAddress));

            // Add the email subject to the message.
            message.Subject = subject;

            // Add the body of the email to the message.
            message.Body = new TextPart ("plain") 
            {
                Text = body
            };

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
