using CtrlInvest.Domain.Interfaces.Application;
using CtrlInvest.Services.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CtrlInvest.Services.Email
{
    public class CustomEmailSender : ICustomEmailSender
    {
        private readonly EmailSettings _mailSettings;

        public CustomEmailSender(IOptions<EmailSettings> emailSettings)
        {
            _mailSettings = emailSettings.Value;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_mailSettings.Mail);
            mailMessage.To.Add(new MailAddress(email));

            mailMessage.Subject = subject;
            mailMessage.BodyEncoding = Encoding.UTF8;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = message;

            using SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(_mailSettings.Mail, _mailSettings.Password);
            client.Host = _mailSettings.Host;
            client.Port = _mailSettings.Port;
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;

            try
            {
                client.Send(mailMessage);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                return Task.CompletedTask;
            }
        }
    }
}
