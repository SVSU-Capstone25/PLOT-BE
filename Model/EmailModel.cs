namespace Plot.Model
{
    public class EmailSettingsModel
    {
        public string? SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string? SenderSmtpPass { get; set; }
        public string? SenderEmail { get; set; }
        public string? SenderName { get; set; }
    }
}