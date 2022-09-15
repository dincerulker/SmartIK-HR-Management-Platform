using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace SmartIK.Services
{
    public class EmailSender : IEmailSender
    {

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using(SmtpClient smtpClient = new SmtpClient("smtp-mail.outlook.com", 587))
            {
                smtpClient.Credentials = new NetworkCredential("smartik.tr@hotmail.com", "P@ssword.06");
                MailMessage mail = new MailMessage("smartik.tr@hotmail.com", email, subject, htmlMessage);
                mail.IsBodyHtml = true;
                smtpClient.EnableSsl = true;
                await smtpClient.SendMailAsync(mail);
            }
        }
    }
}
