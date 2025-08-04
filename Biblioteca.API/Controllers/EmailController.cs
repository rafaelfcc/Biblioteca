using Biblioteca.Domain.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("esqueci-senha")]
        public async Task<IActionResult> EsqueciSenha([FromBody] string email)
        {
            var token = "código ou link com token"; // gerar token real aqui depois
            var assunto = "Recuperação de Senha";
            var corpo = $"<p>Você solicitou a redefinição de senha. Clique no link abaixo:</p><p><a href=\"https://seudominio.com/resetar?token={token}\">Resetar Senha</a></p>";

            await _emailService.EnviarEmailAsync(email, assunto, corpo);
            return Ok(new { mensagem = "Se o e-mail estiver registrado, você receberá um link de recuperação." });
        }
    }
}
