using System.Net;
using System.Net.Mail;
using WallpaperAppBackend.Model;

namespace WallpaperAppBackend.Services
{
//email update
    public class EmailService
    {
        public static void SendForgotPasswordMail(User user)
        {
            if (user != null && !string.IsNullOrEmpty(user.Email) && !string.IsNullOrEmpty(user.Name))
            {
                var senderEmail = new MailAddress("mh18gamming@gmail.com", "CodeWithSk");
                var reciverEmail = new MailAddress(user.Email, user.Name);

                const string subject = "Reset Your Password";
                string body = $"Click the following link to reset your password: https://localhost/reset-password?uid={user.UserId}";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail.Address, "jmzc rjps pvyj ezzu")
                };
                smtp.EnableSsl = true;
                using var message = new MailMessage(senderEmail, reciverEmail)
                {
                    Subject = subject,
                    Body = body
                };
                smtp.Send(message);
            }
        }
    }

  
}
