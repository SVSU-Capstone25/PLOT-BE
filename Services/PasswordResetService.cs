
using MimeKit.Text;

namespace Plot.Services
{
    public class PasswordResetService
    {
        public PasswordResetService()
        {
        }
    

        public bool IsEmailValid(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }
            if (!email.Contains("@"))
            {
                return false;
            }
            return true;
        }
        

        public string FindNameByEmail(string email)
        {
            //DB implementation here
            return "John Doe";
        }

        public string GenerateResetToken()
        {
            //Generate a random token
            
            return "TOKEM";
        }
    }
}