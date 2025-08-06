using Biblioteca.Domain.Contracts;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;

namespace Biblioteca.Repositories.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var smtpHost = _configuration["EmailSettings:SmtpHost"];
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "25");
            var enableSsl = bool.Parse(_configuration["EmailSettings:EnableSsl"] ?? "false");
            var userName = _configuration["EmailSettings:UserName"];
            var password = _configuration["EmailSettings:Password"];
            var fromEmail = _configuration["EmailSettings:FromEmail"];
            var fromName = _configuration["EmailSettings:FromName"];

            using (var client = new SmtpClient(smtpHost, smtpPort))
            {
                client.EnableSsl = enableSsl;
                client.UseDefaultCredentials = string.IsNullOrEmpty(userName);
                if (!string.IsNullOrEmpty(userName))
                {
                    client.Credentials = new System.Net.NetworkCredential(userName, password);
                }

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail, fromName),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(toEmail);

                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
