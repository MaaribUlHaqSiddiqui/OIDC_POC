using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Helpers.EmailSender
{
    public class EmailService : IEmailService
    {
        private readonly string _senderEmail;
        private readonly string _senderPassword;
        private readonly string _smtpHost;
        private readonly int _smtpPort;

        public EmailService(string senderEmail, string senderPassword, string smtpHost = "smtp.office365.com", int smtpPort = 587)
        {
            _senderEmail = senderEmail;
            _senderPassword = senderPassword;
            _smtpHost = smtpHost;
            _smtpPort = smtpPort;
        }

        public void SendHTMLRenderedEmail(string toEmail, string subject, string body)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(toEmail);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true; // Important: This ensures the email is treated as HTML
                mail.From = new MailAddress(_senderEmail);

                SmtpClient smtp = new SmtpClient(_smtpHost); // Replace with your SMTP server
                smtp.Port = _smtpPort; // Adjust if necessary
                smtp.Credentials = new NetworkCredential(_senderEmail, _senderPassword); // Replace with your credentials
                smtp.EnableSsl = true;

                smtp.Send(mail);
                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            
            using (var mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(_senderEmail);
                mailMessage.To.Add(to);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = false;

                using (var smtpClient = new SmtpClient(_smtpHost, _smtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(_senderEmail, _senderPassword);
                    smtpClient.EnableSsl = true;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
        }
    }
}





