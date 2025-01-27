using MailKit.Net.Smtp;
using MimeKit;

//Curently under development. To be used as a Dependency Injection. More comments to come.
namespace EmailService {
    class EmailSender
    {
        private readonly string senderName= string.Empty;
        private readonly string senderEmail= string.Empty;
        private readonly string smtpServer= string.Empty;
        private readonly int smtpPort= 0;
        private readonly string senderSmtpPass= string.Empty;

        public EmailSender(IConfiguration config) 
        {
            var serverEmailSettings=config.GetSection("EmailSettings");

            senderName=serverEmailSettings["SenderName"]?? throw new ArgumentNullException("Sender Name not configured in EmailSettings");
            senderEmail=serverEmailSettings["SenderEmail"]?? throw new ArgumentNullException("Sender Email not configured in EmailSettings");
            smtpServer=serverEmailSettings["SmtpServer"]?? throw new ArgumentNullException("Smtp Server not configured in EmailSettings");
            smtpPort=int.Parse(serverEmailSettings["SmtpPort"]?? throw new ArgumentNullException("Smtp Port Number not configured in EmailSettings"));
            senderSmtpPass=serverEmailSettings["SenderSmtpPass"]?? throw new ArgumentNullException("Sender Smtp password not configured in EmailSettings");
        }
        public async Task SendEmail(string reciverName, string reciverEmailAddress, string subject, string body)
        {
            var message = new MimeMessage ();
            message.From.Add (new MailboxAddress (senderName, senderEmail));
            message.To.Add (new MailboxAddress (reciverName, reciverEmailAddress));
            message.Subject = subject;

            message.Body = new TextPart ("plain") 
            {
                Text = body
            };

            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(senderEmail, senderSmtpPass);

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
