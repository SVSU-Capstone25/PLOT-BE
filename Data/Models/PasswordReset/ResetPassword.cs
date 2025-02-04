namespace Plot.Data.Models.PasswordReset
{
    public class ResetPassword
    {
        public required string NewPassword { get; set; }
        public required string Token { get; set; }
    }
}
