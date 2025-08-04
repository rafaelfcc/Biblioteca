using Biblioteca.Domain.Contracts;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Repositories.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task EnviarEmailAsync(string destinatario, string assunto, string corpoHtml)
        {
            var remetente = _config["EmailSettings:Remetente"];
            var senha = _config["EmailSettings:Senha"];
            var smtp = _config["EmailSettings:Smtp"];
            var porta = int.Parse(_config["EmailSettings:Porta"]);

            using var client = new SmtpClient(smtp, porta)
            {
                Port = porta,
                Credentials = new NetworkCredential(remetente, senha),
                EnableSsl = true
            };

            var mail = new MailMessage
            {
                From = new MailAddress(remetente),
                Subject = assunto,
                Body = corpoHtml,
                IsBodyHtml = true
            };

            mail.To.Add(destinatario);

            await client.SendMailAsync(mail);
        }
    }
}
