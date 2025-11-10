using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Membership.Interfaces;

namespace Infrastructure.Persistence.Gateways
{
    public class EmailNotifierSmtp : IEmailNotifier
    {
        public async Task SendAsync(string toEmail, string subject, string body)
        {
            using var client = new SmtpClient("smtp.yourserver.com");
            var mail = new MailMessage("noreply@yourapp.com", toEmail, subject, body);
            await client.SendMailAsync(mail);
        }
    }
}
