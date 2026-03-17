using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using ThreeCSchool.Core.Service.Abstraction.Services.Auth.Email;
using ThreeCSchool.Shared.Settings;

namespace ThreeCSchool.Core.Service.Services.Auth.Email
{
    public class EmailService(IOptions<EmailSettings> _emailSettings) : IEmailService
    {
        private readonly EmailSettings _settings = _emailSettings.Value;

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var mail = new MailMessage
            {
                From = new MailAddress(_settings.From),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mail.To.Add(to);

            using var smtp = new SmtpClient(_settings.SmtpServer, _settings.SmtpPort)
            {
                Credentials = new NetworkCredential(_settings.UserName, _settings.Password),
                EnableSsl = true
            };

            await smtp.SendMailAsync(mail);
        }
    }
}