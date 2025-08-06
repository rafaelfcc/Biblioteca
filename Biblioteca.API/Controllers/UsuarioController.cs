using Biblioteca.Data.Repositories;
using Biblioteca.Domain.Contracts;
using Biblioteca.Domain.Entities;
using Biblioteca.Repositories.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioRepository _repository;
        private readonly IEmailService _emailService;

        public UsuarioController(UsuarioRepository repository, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }

        [HttpGet("{email}")]
        public ActionResult<Usuario> Get(string email)
        {
            var usuario = _repository.GetList(u => u.EmailLogin == email).FirstOrDefault();

            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }

        [HttpGet]
        public ActionResult<List<Usuario>> GetAll()
        {
            var usuarios = _repository.GetList(u => true);
            return Ok(usuarios);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Post([FromBody] Usuario novoUsuario)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existente = _repository.GetList(u => u.EmailLogin == novoUsuario.EmailLogin).FirstOrDefault();
            if (existente != null)
                return Conflict("Já existe um usuário com esse e-mail.");

            _repository.Insert(novoUsuario);
            return Ok(new { email = novoUsuario.EmailLogin });
        }

        [HttpPut]
        public IActionResult Put([FromBody] Usuario usuarioAtualizado)
        {
            var existente = _repository.GetList(u => u.EmailLogin == usuarioAtualizado.EmailLogin).FirstOrDefault();
            if (existente == null)
                return NotFound();

            existente.Nome = usuarioAtualizado.Nome;
            existente.DataNascimento = usuarioAtualizado.DataNascimento;
            existente.Senha = usuarioAtualizado.Senha;

            _repository.Update(existente);
            return Ok("Usuário atualizado com sucesso");
        }

        [HttpDelete("{email}")]
        public IActionResult Delete(string email)
        {
            var existente = _repository.GetList(u => u.EmailLogin == email).FirstOrDefault();
            if (existente == null)
                return NotFound();

            return _repository.Delete<string>(email) ? Ok("Usuário excluído com sucesso") : StatusCode(500, "Erro ao excluir");
        }

        [AllowAnonymous]
        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] PasswordResetRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest("O e-mail é obrigatório para a redefinição de senha.");
            }

            // 1. Verificar se o e-mail existe no seu banco de dados de usuários (usando o repositório)
            var userExists = _repository.GetList(u => u.EmailLogin == request.Email).Any();

            if (userExists)
            {
                try
                {
                    var subject = "Redefinição de Senha - Sistema Biblioteca";
                    var messageBody = "<p>Você solicitou Reset de senha</p><p>Sua senha provisória é Ct4Rb99@5z</p>";

                    await _emailService.SendEmailAsync(request.Email, subject, messageBody);
                    Console.WriteLine($"E-mail de redefinição de senha enviado para: {request.Email}");

                    //Atualiza Base
                    var existente = _repository.GetList(u => u.EmailLogin == request.Email).FirstOrDefault();
                    if (existente == null)
                        return NotFound();

                    existente.Senha = "Ct4Rb99@5z";

                    _repository.Update(existente);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao enviar e-mail de redefinição de senha para {request.Email}: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"Tentativa de redefinição de senha para e-mail não registrado: {request.Email}");
            }

            // Sempre retorne OK para evitar enumerar e-mails válidos por tentativas,
            // independentemente se o e-mail foi encontrado ou se o envio falhou.
            return Ok(new { message = "Se o e-mail estiver registrado, você receberá instruções para redefinir sua senha." });
        }
    }

    public class PasswordResetRequest
    {
        public string Email { get; set; } = string.Empty;
    }
}
