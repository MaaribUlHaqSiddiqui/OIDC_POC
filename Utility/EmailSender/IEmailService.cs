using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.EmailSender
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
        void SendHTMLRenderedEmail(string toEmail, string subject, string body);
    }
}
