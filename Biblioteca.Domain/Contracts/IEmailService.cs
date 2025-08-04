using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Domain.Contracts
{
    public interface IEmailService
    {
        Task EnviarEmailAsync(string destinatario, string assunto, string corpoHtml);
    }
}
