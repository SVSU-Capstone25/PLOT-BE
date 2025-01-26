using MailKit.Net.Smtp;
using MimeKit;

//Curently under development. To be used as a Dependency Injection. More comments to come.
namespace EmailService {
    class EmailService
    {
        private readonly string senderName;
        private readonly string senderEmail;
        private readonly string smtpServer;
        private readonly int smtpPort;
        private readonly string senderSmtpPass;

        public Email(IConfiguration config)
        {
            var serverEmailSettings=config.GetSection("EmailSettings");
            senderName=serverEmailSettings["SenderName"];
            senderEmail=serverEmailSettings["SenderEmail"];
            smtpServer=serverEmailSettings["SmtpServer"];
            smtpPort=int.parse(serverEmailSettings["SmtpPort"]);
            senderSmtpPass=serverEmailSettings["SenderSmtpPass"];
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
